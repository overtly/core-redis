using StackExchange.Redis;
using System;

namespace Overt.Core.Redis
{
    public static class SerializeExtensions
    {
        private static BinarySerializer _binarySerializer = new BinarySerializer();
        private static NewtonsoftSerializer _newtonsoftSerializer = new NewtonsoftSerializer();

        /// <summary>
        /// 转换为RedisValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static RedisValue ToRedisValue<T>(this T @object, SerializerType? serializer = null)
        {
            if (@object == null)
                return default(RedisValue);

            if (@object is string)
                return @object as string;

            serializer = serializer ?? RedisManager.SerializerType;
            switch (serializer)
            {
                case SerializerType.Binary:
                    return _binarySerializer.Serialize(@object);
                case SerializerType.Json:
                    return _newtonsoftSerializer.Serialize(@object);
                default:
                    throw new NotSupportedException($"not supported for serializer type [{serializer}]");
            }
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static T ToObject<T>(this RedisValue redisValue, SerializerType? serializer = null)
        {
            if (!redisValue.HasValue)
                return default(T);

            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(redisValue, typeof(string));

            serializer = serializer ?? RedisManager.SerializerType;
            switch (serializer)
            {
                case SerializerType.Binary:
                    return _binarySerializer.Deserialize<T>(redisValue);
                case SerializerType.Json:
                    return _newtonsoftSerializer.Deserialize<T>(redisValue);
                default:
                    throw new NotSupportedException($"not supported for serializer type [{serializer}]");
            }
        }
    }
}
