using System.Net.Http.MessagePack;
using System.Net.Http.Tests.Models;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Unit
{
    public class HttpClientGetExtensionsTest
    {
        private readonly HttpClient _client;
        private readonly FakeHttpMessageHandler _handler;
        private readonly MessagePackSerializerOptions _options;
        private readonly string _uri = "http://localhost/";

        public HttpClientGetExtensionsTest()
        {
            _handler = new FakeHttpMessageHandler();
            _client = new HttpClient(_handler);
            _options = MessagePackSerializerOptions.Standard;
        }

        [Fact]
        public async Task GetFromMessagePackAsync_StringUri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).GetFromMessagePackAsync(_uri, typeof(object), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_StringUri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.GetFromMessagePackAsync((string) null, typeof(object), CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_StringUri_Test()
        {
            _handler.ResponseContent = MessagePackContent.Create(SimpleType.Create(), options: _options);

            var result =
                await _client.GetFromMessagePackAsync(_uri, typeof(SimpleType), _options, CancellationToken.None);

            Assert.Contains(MessagePackDefaults.DefaultMediaTypeHeader, _handler.Request.Headers.Accept);
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Generic_StringUri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).GetFromMessagePackAsync<object>(_uri, CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Generic_StringUri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.GetFromMessagePackAsync<object>((string) null, CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Generic_StringUri_Test()
        {
            _handler.ResponseContent = MessagePackContent.Create(SimpleType.Create(), options: _options);

            var result =
                await _client.GetFromMessagePackAsync<SimpleType>(_uri, _options, CancellationToken.None);

            Assert.NotNull(result);
            result.Verify();
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).GetFromMessagePackAsync(new Uri(_uri), typeof(object), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.GetFromMessagePackAsync((Uri) null, typeof(object), CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Uri_Test()
        {
            _handler.ResponseContent = MessagePackContent.Create(SimpleType.Create(), options: _options);

            var result =
                await _client.GetFromMessagePackAsync(new Uri(_uri), typeof(SimpleType), _options,
                    CancellationToken.None);

            Assert.Contains(MessagePackDefaults.DefaultMediaTypeHeader, _handler.Request.Headers.Accept);
            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Generic_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).GetFromMessagePackAsync<object>(new Uri(_uri), CancellationToken.None));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Generic_Uri_WhenUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.GetFromMessagePackAsync<object>((Uri) null, CancellationToken.None));
            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task GetFromMessagePackAsync_Generic_Uri_Test()
        {
            _handler.ResponseContent = MessagePackContent.Create(SimpleType.Create(), options: _options);

            var result =
                await _client.GetFromMessagePackAsync<SimpleType>(new Uri(_uri), _options, CancellationToken.None);

            Assert.NotNull(result);
            var model = Assert.IsType<SimpleType>(result);
            model.Verify();
        }
    }
}