using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Overt.Core.Redis
{
    public class RedisManager
    {
        private static ConnectionMultiplexer _connectionMultiplexer;

#if ASP_NET_CORE
        internal
#else
        public
#endif
        static void Initialize(Action<RedisManagerOptions> config)
        {
            var option = new RedisManagerOptions();
            config(option);

            SerializerType = option.SerializerType;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(option.ConnectionString);
        }

        /// <summary>
        /// 序列化类型
        /// </summary>
        public static SerializerType SerializerType { get; private set; } = SerializerType.Json;

        /// <summary>
        /// 链接
        /// </summary>
        public static IConnectionMultiplexer Connection
        {
            get
            {
                return CheckIsNull<IConnectionMultiplexer>(_connectionMultiplexer);
            }
        }

        /// <summary>
        /// GetDataBase
        /// </summary>
        public static IDatabase Client
        {
            get
            {
                return Connection.GetDatabase();
            }
        }

        /// <summary>
        /// 异常对象
        /// </summary>
        public static event EventHandler<Exception> OnException;

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="serializerType">默认类型是全局初始化类型</param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, RedisValue> func, SerializerType? serializerType = null, Action<Exception> exceptionAction = null)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (typeof(T).BaseType == typeof(Task))
                throw new ArgumentException("use ExecuteAsync...");

            try
            {
                var result = func(Client);
                if (!result.HasValue)
                    return default(T);
                return result.ToObject<T>(serializerType ?? SerializerType);
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                OnException?.Invoke(Client, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public static T Execute<T>(Func<IDatabase, T> func, Action<Exception> exceptionAction = null)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (typeof(T).BaseType == typeof(Task))
                throw new ArgumentException("use ExecuteAsync...");

            try
            {
                var result = func(Client);
                return result;
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                OnException?.Invoke(Client, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 执行返回redisValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="serializerType"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<RedisValue>> func, SerializerType? serializerType = null, Action<Exception> exceptionAction = null)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (typeof(T).BaseType == typeof(Task))
                throw new ArgumentException("T can't be Task");

            try
            {
                var result = await func(Client);
                if (!result.HasValue)
                    return default(T);
                return result.ToObject<T>(serializerType ?? SerializerType);
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                OnException?.Invoke(Client, ex);
                throw ex;
            }
        }

        /// <summary>
        /// return T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="exceptionAction"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteAsync<T>(Func<IDatabase, Task<T>> func, Action<Exception> exceptionAction = null)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));
            if (typeof(T).BaseType == typeof(Task))
                throw new ArgumentException("T can't be Task");

            try
            {
                var result = await func(Client);
                return result;
            }
            catch (Exception ex)
            {
                exceptionAction?.Invoke(ex);
                OnException?.Invoke(Client, ex);
                throw ex;
            }
        }

        #region Private Method
        /// <summary>
        /// 检查是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static T CheckIsNull<T>(T t) where T : class
        {
            if (t == null)
#if ASP_NET_CORE
                throw new InvalidOperationException("Should call AddRedis extension of IServiceCollection method before any other action");
#else
                throw new InvalidOperationException("Should call Initialize method before any other action");
#endif
            return t;
        }
        #endregion

    }
}
