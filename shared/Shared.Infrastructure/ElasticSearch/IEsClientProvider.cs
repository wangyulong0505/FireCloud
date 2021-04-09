using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure
{
    public interface IEsClientProvider
    {
        ElasticClient GetClient();

        ElasticClient GetClient(string indexName);
    }
}
