using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Models
{
    [MessagePackObject]
    public class ComplexType
    {
        [Key(1)] public SimpleType Inner { get; set; } = default!;

        public static ComplexType Create()
        {
            return new()
            {
                Inner = SimpleType.Create()
            };
        }

        public void Verify()
        {
            Assert.NotNull(Inner);
            Inner.Verify();
        }
    }
}