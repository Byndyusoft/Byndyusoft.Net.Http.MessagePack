using System.IO;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http.MessagePack.Formatting
{
    /// <summary>
    ///     <see cref="MediaTypeFormatter" /> class to handle MessagePack.
    /// </summary>
    public class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="formatter">The <see cref="MessagePackMediaTypeFormatter" /> instance to copy settings from.</param>
        protected internal MessagePackMediaTypeFormatter(MessagePackMediaTypeFormatter formatter)
            : base(formatter)
        {
            SerializerOptions = formatter.SerializerOptions;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter" /> class.
        /// </summary>
        /// <param name="options">Options for running serialization.</param>
        public MessagePackMediaTypeFormatter(MessagePackSerializerOptions? options = null)
        {
            SerializerOptions = options ?? MessagePackDefaults.SerializerOptions;
            SupportedMediaTypes.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationMessagePack);
            SupportedMediaTypes.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationXMessagePack);
        }

        /// <summary>
        ///     Options for running the serialization.
        /// </summary>
        public MessagePackSerializerOptions SerializerOptions { get; }

        /// <inheritdoc />
        public override async Task<object?> ReadFromStreamAsync(Type type, Stream readStream, HttpContent? content,
            IFormatterLogger? formatterLogger, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(type, nameof(type));
            Guard.NotNull(readStream, nameof(readStream));

            if (content is ObjectContent objectContent)
                return objectContent.Value;

            if (content?.Headers.ContentLength == 0)
                return null;


            return await MessagePackSerializer.DeserializeAsync(type, readStream, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object? value, Stream writeStream,
            HttpContent? content, TransportContext? transportContext, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(type, nameof(type));
            Guard.NotNull(writeStream, nameof(writeStream));

            await MessagePackSerializer.SerializeAsync(type, writeStream, value, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override bool CanReadType(Type type)
        {
            return MessagePackContent.CanSerialize(type);
        }

        /// <inheritdoc />
        public override bool CanWriteType(Type type)
        {
            return MessagePackContent.CanSerialize(type);
        }
    }
}