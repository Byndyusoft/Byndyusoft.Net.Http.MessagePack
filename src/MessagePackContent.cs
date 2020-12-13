using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http.MessagePack
{
    /// <summary>
    ///     Provides HTTP content based on MessagePack.
    /// </summary>
    public sealed class MessagePackContent : HttpContent
    {
        private MessagePackContent(object inputValue, Type inputType, MediaTypeHeaderValue mediaType,
            MessagePackSerializerOptions options)
        {
            if (inputType == null) throw new ArgumentNullException(nameof(inputType));

            if (inputValue != null && !inputType.IsInstanceOfType(inputValue))
                throw new ArgumentException(
                    $"The specified type {inputType} must derive from the specific value's type {inputValue.GetType()}.");

            Value = inputValue;
            ObjectType = inputType;
            Headers.ContentType = mediaType ?? MessagePackDefaults.MediaTypeHeader;
            SerializerOptions = options ?? MessagePackDefaults.SerializerOptions;
        }

        /// <summary>
        ///     Options to control the behavior during serialization.
        /// </summary>
        public MessagePackSerializerOptions SerializerOptions { get; }

        /// <summary>
        ///     Gets the type of the <see cref="Value" />  to be serialized by this instance.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        ///     Gets the value to be serialized and used as the body of the HttpRequestMessage that sends this instance.
        /// </summary>
        public object Value { get; }

        /// <summary>
        ///     Creates a new instance of the <see cref="MessagePackContent" /> class that will contain the
        ///     <see cref="inputValue" />  serialized as MessagePack.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="inputValue">The value to serialize.</param>
        /// <param name="serializerOptions">Options to control the behavior during serialization.</param>
        /// <param name="mediaType">The media type to use for the content.</param>
        /// <returns>A <see cref="MessagePackContent" /> instance.</returns>
        public static MessagePackContent Create<T>(T inputValue,
            MessagePackSerializerOptions serializerOptions = null, MediaTypeHeaderValue mediaType = null)
        {
            return Create(inputValue, typeof(T), serializerOptions, mediaType);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="MessagePackContent" /> class that will contain the
        ///     <see cref="inputValue" />  serialized as MessagePack.
        /// </summary>
        /// <param name="inputValue">The value to serialize.</param>
        /// <param name="inputType">The type of the value to serialize.</param>
        /// <param name="serializerOptions">Options to control the behavior during serialization.</param>
        /// <param name="mediaType">The media type to use for the content.</param>
        /// <returns>A <see cref="MessagePackContent" /> instance.</returns>
        public static MessagePackContent Create(object inputValue, Type inputType,
            MessagePackSerializerOptions serializerOptions = null,
            MediaTypeHeaderValue mediaType = null)
        {
            if (inputType == null) throw new ArgumentNullException(nameof(inputType));

            return new MessagePackContent(inputValue, inputType, mediaType, serializerOptions);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await MessagePackSerializer.SerializeAsync(ObjectType, stream, Value, SerializerOptions)
                .ConfigureAwait(false);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}