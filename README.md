# Core-Redis

<a name="094e941a"></a>
### 项目层次说明
> Overt.Core.Redis v1.0.1


<a name="e0bf0e74"></a>
#### 1. 项目目录
```csharp
|-Serializers                     序列化类文件夹，包含Json/Binary
|
|-RedisManager.cs                 核心类
|
|-RedisManagerOptions.cs          配置对戏那个
|
|-SerializeExtensions.cs          序列化扩展类 ToRedisValue / ToObject
|
|-SerializerType.cs               序列化类型
|
|-ServiceCollectionExtensions.cs  NetCore注入
```

<a name="d81d4a08"></a>
#### 2. 版本及支持
> - Nuget版本：V 1.0.1
> - 框架支持： Framework4.6.1 - NetStandard 2.0


<a name="7aaf7e9e"></a>
#### 3. 项目依赖
> - Framework 4.6.1

```csharp
Newtonsoft.Json 12.0.2  
StackExchange.Redis 2.0.519
```

> - NetStandard 2.0

```csharp
Newtonsoft.Json 12.0.2  
StackExchange.Redis 2.0.519
Microsoft.Extensions.DependencyInjection.Abstractions 2.2.0
```

<a name="ecff77a8"></a>
### 使用
<a name="84f3f6ef"></a>
#### [](https://www.yuque.com/nm1w78/brpp4p/xo2514#ee74eed7)1. Nuget包引用
```csharp
Install-Package Overt.Core.Redis -Version 1.0.1
```

<a name="85470291"></a>
#### [](https://www.yuque.com/nm1w78/brpp4p/xo2514#b3312061)2. 使用
> - Framework4.6.1

```csharp
var connectionString = "127.0.0.1:6379";
RedisManager.Initialize(config => config.ConnectionString = connectionString);

// Set Async
var a = RedisManager.ExecuteAsync(async c => await c.StringSetAsync("testkey", "testKey111")).Result;
// Get Async
var b = RedisManager.ExecuteAsync(async c => await c.StringGetAsync("testkey")).Result;

// Get Sync
var resp = RedisManager.Execute(c => c.StringSet("testkey", "testval"));

// Get 
var val = RedisManager.Execute<string>(c => c.StringGet("testkey"));

var data = new TestModel
{
    Age = 17,
    Name = "hello",
    Items = new List<decimal> { 0M, 1M }
};
var aa = RedisManager.Execute(c => c.StringSet("testkey", data.ToRedisValue(RedisManager.SerializerType)));

var item = RedisManager.Execute<TestModel>(c => c.StringGet("testkey"));
```

> - DotNetCore

```csharp
var connectionString = "127.0.0.1:6379";
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
```

---


