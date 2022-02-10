using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.MessagePack.Formatting;
using System.Net.Http.Tests.Models;
using System.Threading.Tasks;
using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class MessagePackMediaTypeFormatterTest
    {
        private readonly HttpContent _content;
        private readonly TransportContext _context = null;
        private readonly MessagePackMediaTypeFormatter _formatter;
        private readonly IFormatterLogger _logger = null;
        private readonly MessagePackSerializerOptions _serializerOptions;

        public MessagePackMediaTypeFormatterTest()
        {
            _serializerOptions = MessagePackSerializerOptions.Standard;
            _content = new StreamContent(new MemoryStream());
            _formatter = new MessagePackMediaTypeFormatter(_serializerOptions);
        }

        [Fact]
        public void DefaultConstructor()
        {
            // Act
            var formatter = new MessagePackMediaTypeFormatter();

            // Assert
            Assert.NotNull(formatter.SerializerOptions);
        }

        [Fact]
        public void CopyConstructor()
        {
            // Act
            var copy = new MessagePackMediaTypeFormatter(_formatter);

            // Assert
            Assert.Same(_serializerOptions, copy.SerializerOptions);
        }

        [Fact]
        public void ConstructorWithOptions()
        {
            // Arrange
            var options = MessagePackSerializerOptions.Standard;

            // Act
            var formatter = new MessagePackMediaTypeFormatter(options);

            // Assert
            Assert.Same(options, formatter.SerializerOptions);
        }

        [Theory]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(AbstractClass), false)]
        [InlineData(typeof(Dictionary<string, object>), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(SimpleType), true)]
        [InlineData(typeof(ComplexType), true)]
        public void CanReadType_ReturnsFalse_ForAnyUnsupportedModelType(Type modelType, bool expectedCanRead)
        {
            // Act
            var result = _formatter.CanReadType(modelType);

            // Assert
            Assert.Equal(expectedCanRead, result);
        }

        [Theory]
        [InlineData(typeof(IInterface), false)]
        [InlineData(typeof(AbstractClass), false)]
        [InlineData(typeof(Dictionary<string, object>), true)]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(SimpleType), true)]
        [InlineData(typeof(ComplexType), true)]
        public void CanWriteType_ReturnsFalse_ForAnyUnsupportedModelType(Type modelType, bool expectedCanRead)
        {
            // Act
            var result = _formatter.CanWriteType(modelType);

            // Assert
            Assert.Equal(expectedCanRead, result);
        }

        [Theory]
        [InlineData("application/msgpack")]
        [InlineData("application/x-msgpack")]
        public void HasProperSupportedMediaTypes(string mediaType)
        {
            // Assert
            Assert.Contains(mediaType, _formatter.SupportedMediaTypes.Select(content => content.ToString()));
        }

        [Fact]
        public async Task ReadFromStreamAsync_WhenTypeIsNull_ThrowsException()
        {
            // Assert
            var stream = new MemoryStream();

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.ReadFromStreamAsync(null!, stream, _content, _logger));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromStreamAsync_WhenStreamIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.ReadFromStreamAsync(typeof(object), null!, _content, _logger));

            // Assert
            Assert.Equal("readStream", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsNullObject()
        {
            // Assert
            var content = new StreamMessagePackHttpContent();
            await content.WriteObjectAsync<SimpleType>(null, _serializerOptions);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(object), content.Stream, content, _logger);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ZeroContentLength_ReadsNullObject()
        {
            // Assert
            var content = new StreamMessagePackHttpContent();
            content.Headers.ContentLength = 0;

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(object), content.Stream, content, _logger);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsPrimitiveType()
        {
            // Arrange
            var expectedInt = 10;
            var content = new StreamMessagePackHttpContent();
            await content.WriteObjectAsync(expectedInt, _serializerOptions);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(int), content.Stream, content, _logger);

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsSimpleTypes()
        {
            // Arrange
            var content = new StreamMessagePackHttpContent();
            await content.WriteObjectAsync(SimpleType.Create(), _serializerOptions);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(SimpleType), content.Stream, content, _logger);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task ReadFromStreamAsync_ReadsComplexTypes()
        {
            // Arrange
            var input = new ComplexType {Inner = new SimpleType {Property = 10}};
            var content = new StreamMessagePackHttpContent();
            await content.WriteObjectAsync(input, _serializerOptions);

            // Act
            var result = await _formatter.ReadFromStreamAsync(typeof(ComplexType), content.Stream, content, _logger);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<ComplexType>(result);
            Assert.Equal(input.Inner.Property, model.Inner.Property);
        }


        [Fact]
        public async Task WriteToStreamAsync_WhenTypeIsNull_ThrowsException()
        {
            // Assert
            var stream = new MemoryStream();

            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.WriteToStreamAsync(null!, new object(), stream, _content, _context));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task WriteToStreamAsync_WhenStreamIsNull_ThrowsException()
        {
            // Act
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(
                    () => _formatter.WriteToStreamAsync(typeof(object), new object(), null!, _content, _context));

            // Assert
            Assert.Equal("writeStream", exception.ParamName);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesNullObject()
        {
            // Assert
            var content = new StreamMessagePackHttpContent();

            // Act
            await _formatter.WriteToStreamAsync(typeof(object), null, content.Stream, content, _context);

            // Assert
            var result = await content.ReadObjectAsync<SimpleType>(_serializerOptions);
            Assert.Null(result);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesBasicType()
        {
            // Arrange
            var expectedInt = 10;
            var content = new StreamMessagePackHttpContent();

            // Act
            await _formatter.WriteToStreamAsync(typeof(int), expectedInt, content.Stream, content, _context);

            // Assert
            var result = await content.ReadObjectAsync<int>(_serializerOptions);
            Assert.Equal(expectedInt, result);
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesSimplesType()
        {
            // Arrange
            var input = SimpleType.Create();
            var content = new StreamMessagePackHttpContent();

            // Act
            await _formatter.WriteToStreamAsync(typeof(SimpleType), input, content.Stream, content, _context);

            // Assert
            var result = await content.ReadObjectAsync<SimpleType>(_serializerOptions);
            result.Verify();
        }

        [Fact]
        public async Task WriteToStreamAsync_WritesComplexType()
        {
            // Arrange
            var input = ComplexType.Create();
            var content = new StreamMessagePackHttpContent();

            // Act
            await _formatter.WriteToStreamAsync(typeof(ComplexType), input, content.Stream, content, _context);

            // Assert
            var result = await content.ReadObjectAsync<ComplexType>(_serializerOptions);
            result.Verify();
        }

        private interface IInterface
        {
        }

        private abstract class AbstractClass
        {
        }
    }
}