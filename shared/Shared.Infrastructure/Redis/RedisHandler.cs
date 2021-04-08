using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Shared.Infrastructure.Redis
{
    public class RedisHandler : IRedisHandler
    {
        static ConnectionMultiplexer redis = null;
        public static RedisHandler Handler = new RedisHandler();
        IDatabase db = null;

        public void InitConnection(IConfiguration configuration)
        {
            try
            {
                var redisConnection = configuration.GetSection("Redis:RedisConnectionString").Value;
                redis = ConnectionMultiplexer.Connect(redisConnection);
                db = redis.GetDatabase();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public RedisHandler()
        {
            //
        }

        #region redis string operation

        public string Get(string key)
        {
            return db.StringGet(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return await db.StringGetAsync(key);
        }

        public bool Set(string key, string value, int expirySeconds = 0)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return db.StringSet(key, value, TimeSpan.FromSeconds(expirySeconds));
            }
            return false;
        }

        public async Task<bool> SetAsync(string key, string value, int expirySeconds = 0)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return await db.StringSetAsync(key, value, TimeSpan.FromSeconds(expirySeconds));
            }
            return false;
        }

        public T Get<T>(string key) where T : new()
        {
            if(db == null)
            {
                return default;
            }
            var value = db.StringGet(key);
            if(value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task<T> GetAsync<T>(string key) where T : new()
        {
            if (db == null)
            {
                return default;
            }
            var value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }

        public bool Set<T>(string key, T obj, int expirySeconds = 0) where T : new()
        {
            if(db == null)
            {
                return false;
            }
            var json = JsonSerializer.Serialize(obj);
            return db.StringSet(key, json, TimeSpan.FromSeconds(expirySeconds));
        }

        public bool SetList<T>(string key, List<T> list, int db_index = 0) where T : new()
        {
            if (db == null)
            {
                return false;
            }
            var value = JsonSerializer.Serialize(list);
            return db.StringSet(key, value);
        }

        public List<T> GetList<T>(string key, int db_index = 0) where T : new()
        {
            if (db == null)
            {
                return new List<T>();
            }
            if (db.KeyExists(key))
            {
                var value = db.StringGet(key);
                if (!string.IsNullOrEmpty(value))
                {
                    var list = JsonSerializer.Deserialize<List<T>>(value);
                    return list;
                }
                else
                {
                    return new List<T>();
                }
            }
            else
            {
                return new List<T>();
            }
        }

        public bool IsKeyExists(string key, int db_index = 0)
        {
            if (db == null)
            {
                return false;
            }
            if (db.KeyExists(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DelListByLambda<T>(string key, Func<T, bool> func, int db_index = 0) where T : new()
        {
            if (db == null)
            {
                return false;
            }
            if (db.KeyExists(key))
            {
                var value = db.StringGet(key);
                if (!string.IsNullOrEmpty(value))
                {
                    var list = JsonSerializer.Deserialize<List<T>>(value);
                    if (list.Count > 0)
                    {
                        list = list.SkipWhile<T>(func).ToList();
                        value = JsonSerializer.Serialize(list);
                        return db.StringSet(key, value);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<T> GetListByLambda<T>(string key, Func<T, bool> func, int db_index = 0) where T : new()
        {
            if (db == null)
            {
                return new List<T>();
            }
            if (db.KeyExists(key))
            {
                var value = db.StringGet(key);
                if (!string.IsNullOrEmpty(value))
                {
                    var list = JsonSerializer.Deserialize<List<T>>(value);
                    if (list.Count > 0)
                    {
                        list = list.Where(func).ToList();
                        return list;
                    }
                    else
                    {
                        return new List<T>();
                    }
                }
                else
                {
                    return new List<T>();
                }
            }
            else
            {
                return new List<T>();
            }
        }

        #endregion
    }
}
