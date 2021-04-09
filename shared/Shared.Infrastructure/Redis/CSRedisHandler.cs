using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public class CSRedisHandler : IRedisHandler
    {
        public static CSRedisHandler Handler = new CSRedisHandler();
        public void InitConnection(IConfiguration configuration)
        {
            try
            {
                var redisConnection = configuration.GetSection("Redis:RedisConnectionString").Value;
                var csredis = new CSRedis.CSRedisClient(redisConnection);
                RedisHelper.Initialization(csredis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Get(string key)
        {
            return RedisHelper.Get(key);
        }

        public T Get<T>(string key) where T : new()
        {
            return RedisHelper.Get<T>(key);
        } 

        public async Task<string> GetAsync(string key)
        {
            return await RedisHelper.GetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key) where T : new()
        {
            return await RedisHelper.GetAsync<T>(key);
        }

        public bool IsKeyExists(string key, int db_index = 0)
        {
            return RedisHelper.Exists(key);
        }

        public bool Set(string key, string value, int expirySeconds = 0)
        {
            return RedisHelper.Set(key, value, expirySeconds);
        }

        public bool Set<T>(string key, T obj, int expirySeconds = 0) where T : new()
        {
            return RedisHelper.Set(key, obj, expirySeconds);
        }

        public async Task<bool> SetAsync(string key, string value, int expirySeconds = 0)
        {
            return await RedisHelper.SetAsync(key, value, expirySeconds);
        }
    }
}
