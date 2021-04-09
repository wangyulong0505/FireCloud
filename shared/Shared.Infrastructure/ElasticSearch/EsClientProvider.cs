using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Linq;

namespace Shared.Infrastructure
{
    public class EsClientProvider : IEsClientProvider
    {
        private readonly IOptions<EsConfig> _esConfig;
        public EsClientProvider(IOptions<EsConfig> esConfig)
        {
            _esConfig = esConfig;
        }
        
        public ElasticClient GetClient()
        {
            if(_esConfig == null || _esConfig.Value == null || _esConfig.Value.Urls == null || _esConfig.Value.Urls.Count < 1)
            {
                throw new Exception("urls can not be null");
            }
            return GetClient(_esConfig.Value.Urls.ToArray(), "");
        }

        public ElasticClient GetClient(string indexName)
        {
            if(_esConfig == null || _esConfig.Value == null || _esConfig.Value.Urls == null || _esConfig.Value.Urls.Count < 1)
            {
                throw new Exception("urls can not be null");
            }
            return GetClient(_esConfig.Value.Urls.ToArray(), indexName);
        }

        private ElasticClient GetClient(string url, string defaultIndex = "")
        {
            if(string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("url can not be null");
            }
            var uri = new Uri(url);
            var connectionSetting = new ConnectionSettings(uri);
            if(!string.IsNullOrWhiteSpace(defaultIndex))
            {
                connectionSetting.DefaultIndex(defaultIndex);
            }
            return new ElasticClient(connectionSetting);
        }

        private ElasticClient GetClient(string[] urls, string defaultIndex = "")
        {
            if(urls == null || urls.Length < 1)
            {
                throw new Exception("urls can not be null");
            }
            var uris = urls.Select(p => new Uri(p)).ToArray();
            var connectionPool = new SniffingConnectionPool(uris);
            var connectionSetting = new ConnectionSettings(connectionPool);
            if(!string.IsNullOrWhiteSpace(defaultIndex))
            {
                connectionSetting.DefaultIndex(defaultIndex);
            }
            return new ElasticClient(connectionSetting);
        }
    }
}
