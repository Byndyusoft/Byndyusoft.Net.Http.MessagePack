using MessagePack;
using System.IO;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

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
            SupportedMediaTypes.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationXMessagePack);
            SupportedMediaTypes.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationMessagePack);
        }

        /// <summary>
        ///     Options for running the serialization.
        /// </summary>
        public MessagePackSerializerOptions SerializerOptions { get; }

        /// <inheritdoc />
        public override async Task<object?> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger? formatterLogger, CancellationToken cancellationToken = default)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));
            if (content == null) throw new ArgumentNullException(nameof(content));

            return await MessagePackSerializer.DeserializeAsync(type, readStream, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext? transportContext, CancellationToken cancellationToken = default)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));
            if (content == null) throw new ArgumentNullException(nameof(content));

            await MessagePackSerializer.SerializeAsync(writeStream, value, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override bool CanReadType(Type type) => CanSerialize(type);

        /// <inheritdoc />
        public override bool CanWriteType(Type type) => CanSerialize(type);

        private bool CanSerialize(Type type) => !type.IsAbstract && !type.IsInterface;
    }
}