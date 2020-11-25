using System.Net.Http.MessagePack;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class HttpClientPutExtensionsTest
    {
        private readonly HttpClient _client;
        private readonly MessagePackSerializerOptions _options;
        private readonly string _uri = "http://localhost/";

        public HttpClientPutExtensionsTest()
        {
            _client = new HttpClient(new FakeHttpMessageHandler());
            _options = MessagePackSerializerOptions.Standard;
        }

        [Fact]
        public async Task PutAsMessagePackAsync_StringUri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsMessagePackAsync(_uri, new object(), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_StringUri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PutAsMessagePackAsync((string) null, new object()));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_StringUri_WhenOptionsIsNull_UsesMessagePackContentWithDefaultOptions()
        {
            var response = await _client.PutAsMessagePackAsync(_uri, new object(), CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(MessagePackDefaults.SerializerOptions, content.SerializerOptions);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_StringUri_UsesMessagePackContent()
        {
            var response = await _client.PutAsMessagePackAsync(_uri, new object(), _options, CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(_options, content.SerializerOptions);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsMessagePackAsync(new Uri(_uri), new object(), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception =
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    _client.PutAsMessagePackAsync((Uri) null, new object(), CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_UsesMessagePackContent()
        {
            var response =
                await _client.PutAsMessagePackAsync(new Uri(_uri), new object(), _options, CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(_options, content.SerializerOptions);
        }

        [Fact]
        public async Task PutAsMessagePackAsync_Uri_WhenOptionsIsNull_UsesMessagePackContentWithDefaultOptions()
        {
            var response = await _client.PutAsMessagePackAsync(new Uri(_uri), new object(), CancellationToken.None);

            var content = Assert.IsType<MessagePackContent>(response.RequestMessage.Content);
            Assert.Same(MessagePackDefaults.SerializerOptions, content.SerializerOptions);
        }
    }
}