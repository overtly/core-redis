#if ASP_NET_CORE
using Overt.Core.Redis;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, Action<RedisManagerOptions> config)
        {
            RedisManager.Initialize(config);
            services.AddSingleton(RedisManager.Connection);
            services.AddSingleton(RedisManager.Client);

            return services;
        }
    }
}
#endif