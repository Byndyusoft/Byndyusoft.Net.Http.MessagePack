using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Models
{
    [MessagePackObject]
    public class ComplexType
    {
        [Key(1)] public SimpleType Inner { get; set; }

        public static ComplexType Create()
        {
            return new ComplexType
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