using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Redis
{
    public interface IRedisHandler
    {
        string Get(string key);

        Task<string> GetAsync(string key);

        bool Set(string key, string value, int expirySeconds = 0);

        Task<bool> SetAsync(string key, string value, int expirySeconds = 0);

        T Get<T>(string key) where T : new();

        Task<T> GetAsync<T>(string key) where T : new();

        bool Set<T>(string key, T obj, int expirySeconds = 0) where T : new();

        //bool SetList<T>(string key, List<T> list, int db_index = 0) where T : new();

        //List<T> GetList<T>(string key, int db_index = 0) where T : new();

        bool IsKeyExists(string key, int db_index = 0);

        //bool DelListByLambda<T>(string key, Func<T, bool> func, int db_index = 0) where T : new();

        //List<T> GetListByLambda<T>(string key, Func<T, bool> func, int db_index = 0) where T : new();
    }
}
