using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.MessagePack;
using System.Net.Http.Tests.Models;
using System.Threading.Tasks;
using Xunit;

namespace System.Net.Http.Tests.Functional
{
    public class FunctionalTest : MvcTestFixture
    {
        private static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard;

        protected override void ConfigureMvc(IMvcCoreBuilder builder)
        {
            builder.AddMessagePackFormatters(
                options => { options.SerializerOptions = Options; });
        }

        protected override void ConfigureHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(MessagePackDefaults.MediaTypeHeader);
        }

        [Fact]
        public async Task PostAsMessagePackAsync()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            var response = await Client.PostAsMessagePackAsync("/msgpack-formatter", input, Options);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromMessagePackAsync<SimpleType>();

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task PutAsMessagePackAsync()
        {
            // Arrange
            var input = SimpleType.Create();

            // Act
            var response = await Client.PutAsMessagePackAsync("/msgpack-formatter", input, Options);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromMessagePackAsync<SimpleType>();

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);

            model.Verify();
        }

        [Fact]
        public async Task GetFromMessagePackAsync()
        {
            // Act
            var result = await Client.GetFromMessagePackAsync<SimpleType>("/msgpack-formatter", Options);

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);

            model.Verify();
        }
    }
}