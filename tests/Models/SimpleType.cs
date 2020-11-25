using System.IO;
using MessagePack;
using Xunit;

namespace System.Net.Http.Tests.Models
{
    [MessagePackObject]
    public class SimpleType
    {
        [Key(1)] public string Field;

        [Key(0)] public int Property { get; set; }

        [Key(2)] public SeekOrigin Enum { get; set; }

        [Key(3)] public int? Nullable { get; set; }

        [Key(4)] public int[] Array { get; set; }

        public static SimpleType Create()
        {
            return new SimpleType
            {
                Property = 10,
                Enum = SeekOrigin.Current,
                Field = "string",
                Array = new[] {1, 2},
                Nullable = 100
            };
        }

        public void Verify()
        {
            var input = Create();

            Assert.Equal(input.Property, Property);
            Assert.Equal(input.Field, Field);
            Assert.Equal(input.Enum, Enum);
            Assert.Equal(input.Array, Array);
            Assert.Equal(input.Nullable, Nullable);
        }
    }
}