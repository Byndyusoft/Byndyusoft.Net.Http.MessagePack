using MessagePack;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.MessagePack;
using System.Net.Http.Tests.Models;
using System.Threading.Tasks;
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
                MessagePackContent.Create(null!,
                    new object(), MessagePackSerializerOptions.Standard, MessagePackDefaults.MediaTypeHeader));

            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void Create_InputValueInvalidType_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                MessagePackContent.Create(typeof(int),
                    new SimpleType(), MessagePackSerializerOptions.Standard, MessagePackDefaults.MediaTypeHeader));

            Assert.Contains(
                @$"An object of type '{nameof(SimpleType)}' cannot be used with a type parameter of '{nameof(Int32)}'.",
                exception.Message);
        }

        [Fact]
        public void Create_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(typeof(SimpleType), inputValue, _options, _mediaType);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Equal(_mediaType, content.Headers.ContentType);
            Assert.Same(_options, content.SerializerOptions);
        }

        [Fact]
        public void Create_DefaultPropertyValues_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(typeof(SimpleType), inputValue);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Equal(MessagePackDefaults.MediaTypeHeader, content.Headers.ContentType);
            Assert.Same(MessagePackDefaults.SerializerOptions, content.SerializerOptions);
        }

        [Fact]
        public void Create_Generic_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(inputValue, _options, _mediaType);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Equal(_mediaType, content.Headers.ContentType);
            Assert.Same(_options, content.SerializerOptions);
        }

        [Fact]
        public void Create_Generic_DefaultPropertyValues_Test()
        {
            var inputValue = SimpleType.Create();

            var content = MessagePackContent.Create(inputValue);

            Assert.Same(inputValue, content.Value);
            Assert.Same(typeof(SimpleType), content.ObjectType);
            Assert.Equal(MessagePackDefaults.MediaTypeHeader, content.Headers.ContentType);
            Assert.Same(MessagePackDefaults.SerializerOptions, content.SerializerOptions);
        }

        [Fact]
        public async Task ReadAsByteArrayAsync_Test()
        {
            var inputValue = SimpleType.Create();
            var content = MessagePackContent.Create(inputValue, _options, _mediaType);

            var bytes = await content.ReadAsByteArrayAsync();
            await using var stream = new MemoryStream(bytes);

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            model.Verify();
        }

        [Fact]
        public async Task ReadAsByteArrayAsync_NullObject_Test()
        {
            var content = MessagePackContent.Create<SimpleType>(null!, _options, _mediaType);

            var bytes = await content.ReadAsByteArrayAsync();
            await using var stream = new MemoryStream(bytes);

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            Assert.Null(model);
        }

        [Fact]
        public async Task ReadAsStreamArrayAsync_Test()
        {
            var inputValue = SimpleType.Create();
            var content = MessagePackContent.Create(inputValue, _options, _mediaType);

            await using var stream = await content.ReadAsStreamAsync();

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            model.Verify();
        }

        [Fact]
        public async Task ReadAsStreamArrayAsync_NullObject_Test()
        {
            var content = MessagePackContent.Create<SimpleType>(null!, _options, _mediaType);

            await using var stream = await content.ReadAsStreamAsync();

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            Assert.Null(model);
        }

        [Fact]
        public async Task CopyToAsync_Test()
        {
            var inputValue = SimpleType.Create();
            var content = MessagePackContent.Create(inputValue, _options, _mediaType);
            await using var stream = new MemoryStream();

            await content.CopyToAsync(stream);
            stream.Position = 0;

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            model.Verify();
        }

        [Fact]
        public async Task CopyToAsync_NullObject_Test()
        {
            var content = MessagePackContent.Create<SimpleType>(null!, _options, _mediaType);
            await using var stream = new MemoryStream();

            await content.CopyToAsync(stream);
            stream.Position = 0;

            var model = await MessagePackSerializer.DeserializeAsync<SimpleType>(stream, _options);
            Assert.Null(model);
        }
    }
}