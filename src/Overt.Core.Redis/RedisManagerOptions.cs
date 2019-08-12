namespace Overt.Core.Redis
{
    public class RedisManagerOptions
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 默认序列化类型
        /// </summary>
        public SerializerType SerializerType { get; set; } = SerializerType.Json;
    }
}
