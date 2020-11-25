using System.Net.Http.MessagePack;
using System.Net.Http.Tests.Models;
using System.Threading.Tasks;
using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class HttpContentMessagePackExtensionsTests
    {
        private readonly MessagePackSerializerOptions _options = MessagePackSerializerOptions.Standard
            .WithCompression(MessagePackCompression.Lz4Block);

        [Fact]
        public async Task ReadFromMessagePackAsync_NullContent_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpContent) null).ReadFromMessagePackAsync(typeof(object)));

            Assert.Equal("content", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromMessagePackAsync_Generic_NullContent_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpContent) null).ReadFromMessagePackAsync<object>());

            Assert.Equal("content", exception.ParamName);
        }

        [Fact]
        public async Task ReadFromMessagePackAsync_Test()
        {
            var content = MessagePackContent.Create(SimpleType.Create(), _options);

            var model = await content.ReadFromMessagePackAsync(typeof(SimpleType), _options);

            var simpleType = Assert.IsType<SimpleType>(model);
            simpleType.Verify();
        }

        [Fact]
        public async Task ReadFromMessagePackAsync_Generic_Test()
        {
            var content = MessagePackContent.Create(SimpleType.Create(), _options);

            var model = await content.ReadFromMessagePackAsync<SimpleType>(_options);

            model.Verify();
        }
    }
}