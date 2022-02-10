using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Tests.Unit
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpContent ResponseContent { get; set; }

        public HttpRequestMessage Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Request = request;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = request,
                Content = ResponseContent ?? request.Content
            };
            return Task.FromResult(response);
        }
    }
}