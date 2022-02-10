using MessagePack;
using System.Net.Http.MessagePack;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class HttpClientPostExtensionsTest
    {
        private readonly HttpClient _client;
        private readonly MessagePackSerializerOptions _options;
        private readonly string _uri = "http://localhost/";

        public HttpClientPostExtensionsTest()
        {
            _client = new HttpClient(new FakeHttpMessageHandler());
            _options = MessagePackSerializerOptions.Standard;
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null)!.PostAsMessagePackAsync(_uri, new object(), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PostAsMessagePackAsync(((string)null)!, new object(), CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_WhenOptionsIsNull_UsesMessagePackContentWithDefaultOptions()
        {
            var response = await _client.PostAsMessagePackAsync(_uri, new object(), CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(MessagePackDefaults.SerializerOptions, content.SerializerOptions);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_String_UsesMessagePackContent()
        {
            var response = await _client.PostAsMessagePackAsync(_uri, new object(), _options, CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(_options, content.SerializerOptions);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null)!.PostAsMessagePackAsync(new Uri(_uri), new object(), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _client.PostAsMessagePackAsync(((Uri)null)!, new object(), CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_UsesMessagePackContent()
        {
            var response =
                await _client.PostAsMessagePackAsync(new Uri(_uri), new object(), _options, CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(_options, content.SerializerOptions);
        }

        [Fact]
        public async Task PostAsMessagePackAsync_Uri_WhenOptionsIsNull_UsesMessagePackContentWithDefaultOptions()
        {
            var response = await _client.PostAsMessagePackAsync(new Uri(_uri), new object(), CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(MessagePackDefaults.SerializerOptions, content.SerializerOptions);
        }
    }
}