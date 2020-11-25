using System.Net.Http.Headers;
using MessagePack;
using MessagePack.Resolvers;

namespace System.Net.Http.MessagePack
{
    public static class MessagePackDefaults
    {
        public static readonly string DefaultMediaType = MediaTypes.ApplicationXMessagePack;

        public static readonly MediaTypeWithQualityHeaderValue DefaultMediaTypeHeader =
            new MediaTypeWithQualityHeaderValue(DefaultMediaType);

        public static readonly MessagePackSerializerOptions DefaultSerializerOptions =
            MessagePackSerializerOptions.Standard
                .WithResolver(ContractlessStandardResolverAllowPrivate.Instance);

        public static class MediaTypes
        {
            public const string ApplicationMessagePack = "application/msgpack";
            public const string ApplicationXMessagePack = "application/x-msgpack";
        }
    }
}