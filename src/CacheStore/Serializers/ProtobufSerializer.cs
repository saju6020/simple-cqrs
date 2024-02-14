namespace Platform.Infrastructure.CacheStore.Serializers
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using ProtoBuf;
    using StackExchange.Redis;

    /// <summary>Util class to provide all kind of method that is not considering as a service.</summary>
    [ExcludeFromCodeCoverage]
    public class ProtobufSerializer
    {
        /// <summary>Serializes the specified item.</summary>
        /// <typeparam name="T">Protobuf type.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>Return byte array.</returns>
        public static byte[] Serialize<T>(T item)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, item);
                return ms.ToArray();
            }
        }

        /// <summary>Deserializes the specified item.</summary>
        /// <typeparam name="T">Protobuf type.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>T.</returns>
        public static T Deserialize<T>(RedisValue item)
        {
            return Serializer.Deserialize<T>(item);
        }
    }
}
