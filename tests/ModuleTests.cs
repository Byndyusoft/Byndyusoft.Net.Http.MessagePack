#if NET5_0_OR_GREATER

using System.Net.Http.Formatting;
using System.Net.Http.MessagePack;
using System.Net.Http.MessagePack.Formatting;
using Xunit;

namespace System.Net.Http.Tests
{
    public class ModuleTests
    {
        [Fact]
        public void Init_Test()
        {
            // assert
            var writer =
                MediaTypeFormatterCollection.Default.FindWriter(typeof(string), MessagePackDefaults.MediaTypeHeader);
            Assert.NotNull(writer);
            Assert.IsType<MessagePackMediaTypeFormatter>(writer);

            var reader =
                MediaTypeFormatterCollection.Default.FindReader(typeof(string), MessagePackDefaults.MediaTypeHeader);
            Assert.NotNull(reader);
            Assert.IsType<MessagePackMediaTypeFormatter>(reader);
        }
    }
}

#endif