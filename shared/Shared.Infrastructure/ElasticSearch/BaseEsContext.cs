using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.ElasticSearch
{
    public interface IBaseEsContext { }

    public abstract class BaseEsContext<T> : IBaseEsContext where T : class
    {
        protected IEsClientProvider _esClientProvider;
        public abstract string IndexName { get; }
        public BaseEsContext(IEsClientProvider esClientProvider)
        {
            _esClientProvider = esClientProvider;
        }

        public bool InsertMany(List<T> models)
        {
            var client = _esClientProvider.GetClient(IndexName);
            if(!client.Indices.Exists(IndexName).Exists)
            {
                client.CreateIndex<T>(IndexName);
            }
            var response = client.IndexMany(models);
            //var response = client.Bulk(p => p.Index(IndexName).IndexMany(models));
            return response.IsValid;
        }

        public long GetTotalCount()
        {
            var client = _esClientProvider.GetClient(IndexName);
            var search = new SearchDescriptor<T>().MatchAll();
            var response = client.Search<T>(search);
            return response.Total;
        }

        public bool DeleteById(string id)
        {
            var client = _esClientProvider.GetClient(IndexName);
            var response = client.Delete<T>(id);
            return response.IsValid;
        }
    }
}
