using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure.ElasticSearch
{
    public class EsConfig : IOptions<EsConfig>
    {
        public List<string> Urls { get; set; }

        public EsConfig Value => this;
    }
}
