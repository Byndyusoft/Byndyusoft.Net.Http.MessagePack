using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http.MessagePack
{
    /// <summary>
    ///     Contains extension methods to read and then parse the <see cref="HttpContent" /> from MessagePack.
    /// </summary>
    public static class HttpContentMessagePackExtensions
    {
        /// <summary>
        ///     Reads the HTTP content and returns the value that results from deserializing the content as MessagePack in an
        ///     asynchronous operation.
        /// </summary>
        /// <param name="content">The content to read from.</param>
        /// <param name="type">The type of the object to deserialize to and return.</param>
        /// <param name="options">Options to control the behavior during deserialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<object> ReadFromMessagePackAsync(this HttpContent content, Type type,
            MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return ReadFromMessagePackAsyncCore(content, type, options, cancellationToken);
        }

        /// <summary>
        ///     Reads the HTTP content and returns the value that results from deserializing the content as MessagePack in an
        ///     asynchronous operation.
        /// </summary>
        /// <typeparam name="T">The target type to deserialize to.</typeparam>
        /// <param name="content">The content to read from.</param>
        /// <param name="options">Options to control the behavior during deserialization.</param>
        /// <param name="cancellationToken">
        ///     A cancellation token that can be used by other objects or threads to receive notice of
        ///     cancellation.
        /// </param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<T> ReadFromMessagePackAsync<T>(this HttpContent content,
            MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return ReadFromMessagePackAsyncCore<T>(content, options, cancellationToken);
        }

        private static async Task<object> ReadFromMessagePackAsyncCore(HttpContent content, Type type,
            MessagePackSerializerOptions options, CancellationToken cancellationToken)
        {
            using (var contentStream =
                await ReadHttpContentStreamAsync(content, cancellationToken).ConfigureAwait(false))
            {
                return await MessagePackSerializer.DeserializeAsync(type, contentStream, options, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private static async Task<T> ReadFromMessagePackAsyncCore<T>(HttpContent content,
            MessagePackSerializerOptions options, CancellationToken cancellationToken)
        {
            using (var contentStream =
                await ReadHttpContentStreamAsync(content, cancellationToken).ConfigureAwait(false))
            {
                return await MessagePackSerializer.DeserializeAsync<T>(contentStream, options, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private static Task<Stream> ReadHttpContentStreamAsync(HttpContent content, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return content.ReadAsStreamAsync();
        }
    }
}