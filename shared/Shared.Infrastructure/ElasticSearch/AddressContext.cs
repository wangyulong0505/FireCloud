using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Infrastructure
{
    public class AddressContext : BaseEsContext<Address>
    {
        public override string IndexName => "address";
        public AddressContext(IEsClientProvider provider) : base(provider)
        {
            //
        }

        public List<Address> GetAddresses(string province, int pageIndex, int pageSize)
        {
            var client = _esClientProvider.GetClient(IndexName);
            var queryContainers = new List<Func<QueryContainerDescriptor<Address>, QueryContainer>>();
            queryContainers.Add(p => p.Term(m => m.Field(x => x.Pronvince).Value(province)));
            var search = new SearchDescriptor<Address>();
            search = search.Query(p => p.Bool(m => m.Must(queryContainers))).From((pageIndex - 1) * pageSize).Take(pageSize);
            var response = client.Search<Address>(search);
            return response.Documents.ToList();
        }

        public List<Address> GetAllAddresses()
        {
            var client = _esClientProvider.GetClient(IndexName);
            var searchDescriptor = new SearchDescriptor<Address>();
            searchDescriptor = searchDescriptor.Query(p => p.MatchAll());
            var response = client.Search<Address>(searchDescriptor);
            return response.Documents.ToList();
        }

        public bool DeleteByQuery(string city)
        {
            var client = _esClientProvider.GetClient(IndexName);
            var queryContainers = new List<Func<QueryContainerDescriptor<Address>, QueryContainer>>();
            queryContainers.Add(p => p.Term(m => m.Field(f => f.City).Value(city)));
            var search = new DeleteByQueryDescriptor<Address>().Index(IndexName);
            search = search.Query(p => p.Bool(m => m.Must(queryContainers)));
            var response = client.DeleteByQuery<Address>(p=>search);
            return response.IsValid;
        }
    }
}
