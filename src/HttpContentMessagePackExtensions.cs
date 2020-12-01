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
        public static async Task<object> ReadFromMessagePackAsync(this HttpContent content, Type type,
            MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            if (content is MessagePackContent msgPackContent) return msgPackContent.Value;

            using var readStream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            return await MessagePackSerializer.DeserializeAsync(type, readStream, options, cancellationToken)
                .ConfigureAwait(false);
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
        public static async Task<T> ReadFromMessagePackAsync<T>(this HttpContent content,
            MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return (T) await ReadFromMessagePackAsync(content, typeof(T), options, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}