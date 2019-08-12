using Microsoft.Extensions.DependencyInjection;
using Overt.Core.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace ConsoleUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "10.0.12.10:6390,10.0.12.10:6391,10.0.12.10:6392,10.0.12.10:6393,10.0.12.10:6394,10.0.12.10:6395";
            var services = new ServiceCollection();
            services.AddRedis(config => config.ConnectionString = connectionString);

            var provider = services.BuildServiceProvider();
            var connection = provider.GetService<IConnectionMultiplexer>();
            var client = provider.GetService<IDatabase>();

            var a = RedisManager.ExecuteAsync(async c => await c.StringSetAsync("testkey", "testKey")).Result;
            var b = RedisManager.ExecuteAsync(async c => await c.StringGetAsync("testkey")).Result;

            var resp = RedisManager.Execute(c => c.StringSet("testkey", "testval"));

            var val = RedisManager.Execute<string>(c => c.StringGet("testkey"));

            var data = new TestModel
            {
                Age = 17,
                Name = "hello",
                Items = new List<decimal> { 0M, 1M }
            };
            var aa = RedisManager.Execute(c => c.StringSet("testkey", data.ToRedisValue()));

            var item = RedisManager.Execute<TestModel>(c => c.StringGet("testkey"));
            Console.ReadLine();
        }
    }
}
