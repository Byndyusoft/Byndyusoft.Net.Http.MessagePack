using MessagePack;

namespace System.Net.Http.Tests.Models
{
    [MessagePackObject]
    public class ComplexType
    {
        [Key(0)] public SimpleType Inner { get; set; }
    }
}