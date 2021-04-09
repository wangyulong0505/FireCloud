using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public class MongodbHandler
    {
        private static string ConnectionStrings { get; set; }
        public MongodbHandler(List<string> connectionStrings)
        {
            ConnectionStrings = string.Join(',', connectionStrings);
        }

        public static IMongoDatabase GetDatabase(string databaseName)
        {
            var client = new MongoClient(ConnectionStrings);
            return client.GetDatabase(databaseName);
        }

        public static IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
        {
            var client = new MongoClient(ConnectionStrings);
            var database = client.GetDatabase(databaseName);

            return database.GetCollection<T>(collectionName);
        }

        public static bool Add<T>(string databaseName, string collectionName, T t)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                collection.InsertOne(t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> AddAsync<T>(string databaseName, string collectionName, T t)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                await collection.InsertOneAsync(t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddRange<T>(string databaseName, string collectionName, IEnumerable<T> t)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                collection.InsertMany(t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> AddRangeAsync<T>(string databaseName, string collectionName, IEnumerable<T> t)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                await collection.InsertManyAsync(t);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static UpdateResult Update<T>(string databaseName, string collectionName, T t, string id)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                var list = new List<UpdateDefinition<T>>();
                foreach(var item in t.GetType().GetProperties())
                {
                    if(item.Name.ToLower() == "id")
                    {
                        continue;
                    }
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return collection.UpdateOne(filter, updateFilter);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<UpdateResult> UpdateAsync<T>(string databaseName, string collectionName, T t, string id)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "id")
                    {
                        continue;
                    }
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return await collection.UpdateOneAsync(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UpdateResult UpdateRange<T>(string databaseName, string collectionName, Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                T t = new T();
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name))
                    {
                        continue;
                    }
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return collection.UpdateMany(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<UpdateResult> UpdateRangeAsync<T>(string databaseName, string collectionName, Dictionary<string, string> dic, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                T t = new T();
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dic.ContainsKey(item.Name))
                    {
                        continue;
                    }
                    var value = dic[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return await collection.UpdateManyAsync(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteResult Delete<T>(string databaseName, string collectionName, string id)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                return collection.DeleteOne(filter);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<DeleteResult> DeleteAsync<T>(string databaseName, string collectionName, string id)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                return await collection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteResult DeleteRange<T>(string databaseName, string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                return collection.DeleteMany(filter);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<DeleteResult> DeleteRangeAsync<T>(string databaseName, string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                return await collection.DeleteManyAsync(filter);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static T Select<T>(string databaseName, string collectionName, string id, IEnumerable<string> field = null)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                if (field == null || field.Count() == 0)
                {
                    return collection.Find(filter).FirstOrDefault();
                }

                var fieldList = new List<ProjectionDefinition<T>>();
                foreach(var item in field)
                {
                    fieldList.Add(Builders<T>.Projection.Include(item));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return collection.Find(filter).Project<T>(projection).FirstOrDefault<T>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<T> SelectAsync<T>(string databaseName, string collectionName, string id, IEnumerable<string> field = null)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                if (field == null || field.Count() == 0)
                {
                    return await collection.Find(filter).FirstOrDefaultAsync();
                }

                var fieldList = new List<ProjectionDefinition<T>>();
                foreach (var item in field)
                {
                    fieldList.Add(Builders<T>.Projection.Include(item));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return await collection.Find(filter).Project<T>(projection).FirstOrDefaultAsync<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> SelectRange<T>(string databaseName, string collectionName, FilterDefinition<T> filter, IEnumerable<string> field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                if (field == null || field.Count() == 0)
                {
                    if (sort == null)
                    {
                        return collection.Find(filter).ToList();
                    }
                    return collection.Find(filter).Sort(sort).ToList();
                }

                var fieldList = new List<ProjectionDefinition<T>>();
                foreach (var item in field)
                {
                    fieldList.Add(Builders<T>.Projection.Include(item));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null)
                {
                    return collection.Find(filter).Project<T>(projection).ToList();
                }
                return collection.Find(filter).Sort(sort).Project<T>(projection).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<T>> SelectRangeAsync<T>(string databaseName, string collectionName, FilterDefinition<T> filter, IEnumerable<string> field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                if (field == null || field.Count() == 0)
                {
                    if (sort == null)
                    {
                        return await collection.Find(filter).ToListAsync();
                    }
                    return await collection.Find(filter).Sort(sort).ToListAsync();
                }

                var fieldList = new List<ProjectionDefinition<T>>();
                foreach (var item in field)
                {
                    fieldList.Add(Builders<T>.Projection.Include(item));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null)
                {
                    return await collection.Find(filter).Project<T>(projection).ToListAsync();
                }
                return await collection.Find(filter).Sort(sort).Project<T>(projection).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> SelectRangeByPage<T>(string databaseName, string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, out long count, IEnumerable<string> field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                count = collection.CountDocuments(filter);
                if (pageIndex < 1 || pageSize < 1)
                {
                    return null;
                }
                if (field == null || field.Count() == 0)
                {
                    if (sort == null)
                    {
                        return collection.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                    }
                    return collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                }

                var fieldList = new List<ProjectionDefinition<T>>();
                foreach (var item in field)
                {
                    fieldList.Add(Builders<T>.Projection.Include(item));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                if (sort == null)
                {
                    return collection.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                }

                return collection.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<T>> SelectRangeByPageAsync<T>(string databaseName, string collectionName, FilterDefinition<T> filter, int pageIndex, int pageSize, IEnumerable<string> field = null, SortDefinition<T> sort = null)
        {
            try
            {
                var collection = GetCollection<T>(databaseName, collectionName);
                //var count = collection.CountDocuments(filter);
                if (pageIndex < 1 || pageSize < 1)
                {
                    return null;
                }
                if (field == null || field.Count() == 0)
                {
                    if (sort == null)
                    {
                        return await collection.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                    }
                    return await collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }

                var fieldList = new List<ProjectionDefinition<T>>();
                foreach (var item in field)
                {
                    fieldList.Add(Builders<T>.Projection.Include(item));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();

                if (sort == null)
                {
                    return await collection.Find(filter).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
                }
                return await collection.Find(filter).Sort(sort).Project<T>(projection).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
