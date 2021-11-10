using MessagePack;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.MessagePack
{
    /// <summary>
    ///     Contains the extensions methods for using MessagePack as the content-type in HttpClient.
    /// </summary>
    public static partial class HttpClientMessagePackExtensions
    {
        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="type">The type of the object to deserialize to and return.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<object?> GetFromMessagePackAsync(this HttpClient client, string requestUri, Type type,
            MessagePackSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            requestMessage.Headers.Accept.Add(MessagePackDefaults.MediaTypeHeader);

            var taskResponse = client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            return GetFromMessagePackAsyncCore(taskResponse, type, options, cancellationToken);
        }

        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="type">The type of the object to deserialize and return to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<object?> GetFromMessagePackAsync(this HttpClient client, string requestUri, Type type,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.GetFromMessagePackAsync(requestUri, type, null, cancellationToken);
        }

        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="type">The type of the object to deserialize to and return.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<object?> GetFromMessagePackAsync(this HttpClient client, Uri requestUri, Type type,
            MessagePackSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            requestMessage.Headers.Accept.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationMessagePack);
            requestMessage.Headers.Accept.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationXMessagePack);

            var taskResponse = client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            return GetFromMessagePackAsyncCore(taskResponse, type, options, cancellationToken);
        }

        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="type">The type of the object to deserialize and return to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<object?> GetFromMessagePackAsync(this HttpClient client, Uri requestUri, Type type,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.GetFromMessagePackAsync(requestUri, type, null, cancellationToken);
        }

        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<TValue> GetFromMessagePackAsync<TValue>(this HttpClient client, string requestUri,
            MessagePackSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            requestMessage.Headers.Accept.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationMessagePack);
            requestMessage.Headers.Accept.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationXMessagePack);

            var taskResponse = client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            return GetFromMessagePackAsyncCore<TValue>(taskResponse, options, cancellationToken);
        }

        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<TValue> GetFromMessagePackAsync<TValue>(this HttpClient client, string requestUri,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.GetFromMessagePackAsync<TValue>(requestUri, null, cancellationToken);
        }

        /// <summary>
        ///     Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as
        ///     MessagePack in an asynchronous operation.
        /// </summary>
        /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<TValue> GetFromMessagePackAsync<TValue>(this HttpClient client, Uri requestUri,
            MessagePackSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            requestMessage.Headers.Accept.Add(MessagePackDefaults.MediaTypeHeader);

            var taskResponse = client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead,
                cancellationToken);
            return GetFromMessagePackAsyncCore<TValue>(taskResponse, options, cancellationToken);
        }

        /// <summary>
        ///     Send a GET request  to the specified Uri and return the value resulting from deserialize the response body
        ///     as MessagePack in an asynchronous operation.
        /// </summary>
        /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<TValue> GetFromMessagePackAsync<TValue>(this HttpClient client, Uri requestUri,
            CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.GetFromMessagePackAsync<TValue>(requestUri, null, cancellationToken);
        }

        private static async Task<object?> GetFromMessagePackAsyncCore(Task<HttpResponseMessage> taskResponse, Type type,
            MessagePackSerializerOptions? options, CancellationToken cancellationToken)
        {
            using var response = await taskResponse.ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromMessagePackAsync(type, options, cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<T> GetFromMessagePackAsyncCore<T>(Task<HttpResponseMessage> taskResponse,
            MessagePackSerializerOptions? options, CancellationToken cancellationToken)
        {
            using var response = await taskResponse.ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromMessagePackAsync<T>(options, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}