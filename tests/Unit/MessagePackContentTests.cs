using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.MessagePack;
using System.Net.Http.Tests.Models;
using System.Threading.Tasks;
using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class MessagePackContentTests
    {
        private readonly MediaTypeHeaderValue _mediaType = MediaTypeHeaderValue.Parse("application/media-type");

        private readonly MessagePackSerializerOptions _options = MessagePackSerializerOptions.Standard
            .WithCompression(MessagePackCompression.Lz4Block);

        [Fact]
        public void Create_NullInputType_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                MessagePackContent.Create(new object(), null, MessagePackDefaults.DefaultMediaTypeHeader,
                    MessagePackSerializerOptions.Standard));

            Assert.Equal("inputType", exception.ParamName);
        }

        [Fact]
        public void Create_InputValueInvalidType_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                MessagePackContent.Create(new SimpleType(), typeof(int), MessagePackDefaults.DefaultMediaTypeHeader,
                    MessagePackSerializerOptions.Standard));

            Assert.Equal(
                $"The specified type {typeof(int)} must derive from the specific value's type {typeof(SimpleType)}.",
                exception.Message);
        }

        [Fact]
        public void Create_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(inputValue, typeof(SimpleType), _mediaType, _options);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Same(_mediaType, content.Headers.ContentType);
            Assert.Same(_options, content.Options);
        }

        [Fact]
        public void Create_Default_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(inputValue, typeof(SimpleType));

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Same(MessagePackDefaults.DefaultMediaTypeHeader, content.Headers.ContentType);
            Assert.Same(MessagePackDefaults.DefaultSerializerOptions, content.Options);
        }

        [Fact]
        public void Create_Generic_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(inputValue, _mediaType, _options);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Same(_mediaType, content.Headers.ContentType);
            Assert.Same(_options, content.Options);
        }

        [Fact]
        public void Create_Generic_Default_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(inputValue);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Same(MessagePackDefaults.DefaultMediaTypeHeader, content.Headers.ContentType);
            Assert.Same(MessagePackDefaults.DefaultSerializerOptions, content.Options);
        }

        [Fact]
        public async Task ReadAsByteArrayAsync_Test()
        {
            var inputValue = SimpleType.Create();
            var content = MessagePackContent.Create(inputValue, _mediaType, _options);

            var bytes = await content.ReadAsByteArrayAsync();
            await using var stream = new MemoryStream(bytes);

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            model.Verify();
        }

        [Fact]
        public async Task ReadAsStreamArrayAsync_Test()
        {
            var inputValue = SimpleType.Create();
            var content = MessagePackContent.Create(inputValue, _mediaType, _options);

            await using var stream = await content.ReadAsStreamAsync();

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            model.Verify();
        }

        [Fact]
        public async Task CopyToAsync_Test()
        {
            var inputValue = SimpleType.Create();
            var content = MessagePackContent.Create(inputValue, _mediaType, _options);
            await using var stream = new MemoryStream();

            await content.CopyToAsync(stream);
            stream.Position = 0;


            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            model.Verify();
        }
    }
}