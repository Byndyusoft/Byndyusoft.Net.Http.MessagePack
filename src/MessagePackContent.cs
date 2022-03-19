using System.Net.Http.Headers;
using System.Net.Http.MessagePack.Formatting;
using MessagePack;

namespace System.Net.Http.MessagePack
{
    /// <summary>
    ///     Provides HTTP content based on MessagePack.
    /// </summary>
    public sealed class MessagePackContent : ObjectContent
    {
        private MessagePackContent(Type type, object? value, MessagePackMediaTypeFormatter formatter,
            MediaTypeHeaderValue? mediaType)
            : base(type, value, formatter, mediaType)
        {
            SerializerOptions = formatter.SerializerOptions;
        }

        /// <summary>
        ///     Options to control the behavior during serialization.
        /// </summary>
        public MessagePackSerializerOptions SerializerOptions { get; }

        /// <summary>
        ///     Creates a new instance of the <see cref="MessagePackContent" /> class that will contain the
        ///     <see cref="value" />  serialized as MessagePack.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="value">The value to serialize.</param>
        /// <param name="serializerOptions">Options to control the behavior during serialization.</param>
        /// <param name="mediaType">The media type to use for the content.</param>
        /// <returns>A <see cref="MessagePackContent" /> instance.</returns>
        public static MessagePackContent Create<T>(T value,
            MessagePackSerializerOptions? serializerOptions = null, MediaTypeHeaderValue? mediaType = null)
        {
            return Create(typeof(T), value, serializerOptions, mediaType);
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="MessagePackContent" /> class that will contain the
        ///     <see cref="value" />  serialized as MessagePack.
        /// </summary>
        /// <param name="type">The type of the value to serialize.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="serializerOptions">Options to control the behavior during serialization.</param>
        /// <param name="mediaType">The media type to use for the content.</param>
        /// <returns>A <see cref="MessagePackContent" /> instance.</returns>
        public static MessagePackContent Create(Type type,
            object? value,
            MessagePackSerializerOptions? serializerOptions = null,
            MediaTypeHeaderValue? mediaType = null)
        {
            Guard.NotNull(type, nameof(type));

            var formatter = new MessagePackMediaTypeFormatter(serializerOptions);

            return new MessagePackContent(type, value, formatter, mediaType);
        }

        public static bool CanSerialize(Type type)
        {
            Guard.NotNull(type, nameof(type));

            return !type.IsAbstract && !type.IsInterface;
        }
    }
}