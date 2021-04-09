using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Shared.Infrastructure
{
    public class EsConfig : IOptions<EsConfig>
    {
        public List<string> Urls { get; set; }

        public EsConfig Value => this;
    }
}
