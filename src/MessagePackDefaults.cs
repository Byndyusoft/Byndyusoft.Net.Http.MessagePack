using System.Net.Http.Headers;
using MessagePack;
using MessagePack.Resolvers;

namespace System.Net.Http.MessagePack
{
    public static class MessagePackDefaults
    {
        public static readonly string MediaTypeFormat = "msgpack";

        public static readonly string MediaType = MediaTypes.ApplicationXMessagePack;

        public static readonly MediaTypeWithQualityHeaderValue MediaTypeHeader =
            new MediaTypeWithQualityHeaderValue(MediaType);

        public static readonly MessagePackSerializerOptions SerializerOptions =
            MessagePackSerializerOptions.Standard
                .WithResolver(ContractlessStandardResolverAllowPrivate.Instance);

        public static class MediaTypes
        {
            public const string ApplicationMessagePack = "application/msgpack";
            public const string ApplicationXMessagePack = "application/x-msgpack";
        }
    }
}