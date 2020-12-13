using System.IO;
using System.Net.Http.MessagePack;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     <see cref="MediaTypeFormatter" /> class to handle MessagePack.
    /// </summary>
    public class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackMediaTypeFormatter" /> class.
        /// </summary>
        public MessagePackMediaTypeFormatter() : this(MessagePackDefaults.SerializerOptions)
        {
        }

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
        public MessagePackMediaTypeFormatter(MessagePackSerializerOptions options)
        {
            SerializerOptions = options ?? throw new ArgumentNullException(nameof(options));
            SupportedMediaTypes.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationXMessagePack);
            SupportedMediaTypes.Add(MessagePackDefaults.MediaTypeHeaders.ApplicationMessagePack);
        }

        /// <summary>
        ///     Options for running the serialization.
        /// </summary>
        public MessagePackSerializerOptions SerializerOptions { get; }

        /// <inheritdoc />
        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (readStream is null) throw new ArgumentNullException(nameof(readStream));
            if (content == null) throw new ArgumentNullException(nameof(content));

            return await content.ReadFromMessagePackAsync(type, SerializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            return ReadFromStreamAsync(type, readStream, content, formatterLogger, CancellationToken.None);
        }

        /// <inheritdoc />
        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext,
            CancellationToken cancellationToken)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (writeStream is null) throw new ArgumentNullException(nameof(writeStream));
            if (content == null) throw new ArgumentNullException(nameof(content));

            var messagePackContent = content as MessagePackContent ??
                                     MessagePackContent.Create(value, type, SerializerOptions);
            await messagePackContent.CopyToAsync(writeStream).ConfigureAwait(false);
            content.Headers.ContentLength = messagePackContent.Headers.ContentLength;
        }

        /// <inheritdoc />
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            return WriteToStreamAsync(type, value, writeStream, content, transportContext, CancellationToken.None);
        }

        /// <inheritdoc />
        public override bool CanReadType(Type type)
        {
            return CanSerialize(type);
        }

        /// <inheritdoc />
        public override bool CanWriteType(Type type)
        {
            return CanSerialize(type);
        }

        private bool CanSerialize(Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }
    }
}