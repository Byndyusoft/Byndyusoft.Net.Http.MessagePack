using System.Net.Http.Headers;
using MessagePack;
using MessagePack.Resolvers;

namespace System.Net.Http.MessagePack
{
    public static class MessagePackDefaults
    {
        public static readonly string MediaTypeFormat = "msgpack";

        public static readonly string MediaType = MediaTypes.ApplicationMessagePack;

        public static readonly MediaTypeWithQualityHeaderValue MediaTypeHeader =
            new(MediaType);

        public static readonly MessagePackSerializerOptions SerializerOptions =
            MessagePackSerializerOptions.Standard
                .WithResolver(ContractlessStandardResolverAllowPrivate.Instance);

        public static class MediaTypeHeaders
        {
            public static readonly MediaTypeWithQualityHeaderValue ApplicationMessagePack =
                new(MediaTypes.ApplicationMessagePack);

            public static readonly MediaTypeWithQualityHeaderValue ApplicationXMessagePack =
                new(MediaTypes.ApplicationXMessagePack);
        }

        public static class MediaTypes
        {
            public const string ApplicationMessagePack = "application/msgpack";
            public const string ApplicationXMessagePack = "application/x-msgpack";
        }
    }
}