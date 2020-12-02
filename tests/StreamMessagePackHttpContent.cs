using System.IO;
using System.Threading.Tasks;
using MessagePack;

namespace System.Net.Http.Tests
{
    public class StreamMessagePackHttpContent : StreamContent
    {
        public StreamMessagePackHttpContent() : this(new MemoryStream())
        {
        }

        private StreamMessagePackHttpContent(MemoryStream stream) : base(stream)
        {
            Stream = stream;
        }

        public MemoryStream Stream { get; }

        public async Task WriteObjectAsync<T>(T value, MessagePackSerializerOptions serializerOptions)
        {
            await MessagePackSerializer.SerializeAsync(typeof(T), Stream, value, serializerOptions)
                .ConfigureAwait(false);
            Stream.Position = 0;
        }

        public async Task<T> ReadObjectAsync<T>(MessagePackSerializerOptions serializerOptions)
        {
            Stream.Position = 0;
            return await MessagePackSerializer.DeserializeAsync<T>(Stream, serializerOptions)
                .ConfigureAwait(false);
        }
    }
}