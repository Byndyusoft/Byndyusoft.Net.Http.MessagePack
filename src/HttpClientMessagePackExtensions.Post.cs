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
        ///     Send a POST request to the specified Uri containing the <paramref name="value" /> serialized as MessagePack in the
        ///     request body.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsMessagePackAsync<TValue>(this HttpClient client,
            string requestUri, TValue value, MessagePackSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var content = MessagePackContent.Create(value, options);
            return client.PostAsync(requestUri, content, cancellationToken);
        }

        /// <summary>
        ///     Send a POST request to the specified Uri containing the <paramref name="value" /> serialized as MessagePack in the
        ///     request body.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsMessagePackAsync<TValue>(this HttpClient client,
            string requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.PostAsMessagePackAsync(requestUri, value, null, cancellationToken);
        }

        /// <summary>
        ///     Send a POST request to the specified Uri containing the <paramref name="value" /> serialized as MessagePack in the
        ///     request body.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="options">Options for running the serialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsMessagePackAsync<TValue>(this HttpClient client, Uri requestUri,
            TValue value, MessagePackSerializerOptions? options, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var content = MessagePackContent.Create(value, options);
            return client.PostAsync(requestUri, content, cancellationToken);
        }

        /// <summary>
        ///     Send a POST request to the specified Uri containing the <paramref name="value" /> serialized as MessagePack in the
        ///     request body.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsMessagePackAsync<TValue>(this HttpClient client, Uri requestUri,
            TValue value, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            return client.PostAsMessagePackAsync(requestUri, value, null, cancellationToken);
        }
    }
}