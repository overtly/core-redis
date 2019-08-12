using Overt.Core.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "120.27.245.101:6379";
            RedisManager.Initialize(config => config.ConnectionString = connectionString);

            var a = RedisManager.ExecuteAsync(async c => await c.StringSetAsync("testkey", "testKey111")).Result;
            var b = RedisManager.ExecuteAsync(async c => await c.StringGetAsync("testkey")).Result;

            var resp = RedisManager.Execute(c => c.StringSet("testkey", "testval"));

            var val = RedisManager.Execute<string>(c => c.StringGet("testkey"));

            var data = new TestModel
            {
                Age = 17,
                Name = "hello",
                Items = new List<decimal> { 0M, 1M }
            };

            var aa = RedisManager.Execute(c => c.StringSet("testkey", data.ToRedisValue(RedisManager.SerializerType)));

            var item = RedisManager.Execute<TestModel>(c => c.StringGet("testkey"));
            Console.ReadLine();
        }
    }
}
