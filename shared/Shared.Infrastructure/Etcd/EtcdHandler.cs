using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using dotnet_etcd;
using Etcdserverpb;
using Google.Protobuf;

namespace Shared.Infrastructure.Etcd
{
    public class EtcdHandler
    {
        private static EtcdClient Client { get; set; }
        public EtcdHandler(List<string> etcdClientStrings)
        {
            var clientUrls = string.Join(',', etcdClientStrings);
            Client = new EtcdClient(clientUrls);
        }

        public static string GetToken(string name, string password)
        {
            var authRes = Client.Authenticate(new Etcdserverpb.AuthenticateRequest()
            {
                Name = name,
                Password = password,
            });
            return authRes.Token;
        }

        public static PutResponse Add(string key, string value)
        {
            return Client.Put(key, value);
        }

        public static async Task<PutResponse> AddAsync(string key, string value)
        {
            return await Client.PutAsync(key, value);
        }

        public static RangeResponse Get(string key)
        {
            return Client.Get(key);
        }

        public static async Task<RangeResponse> GetAsync(string key)
        {
            return await Client.GetAsync(key);
        }

        public static string GetValue(string key)
        {
            return Client.GetVal(key);
        }

        public static async Task<string> GetValueAsync(string key)
        {
            return await Client.GetValAsync(key);
        }

        public static RangeResponse GetRange(string prefixKey)
        {
            return Client.GetRange(prefixKey);
        }

        public static async Task<RangeResponse> GetRangeAsync(string prefixKey)
        {
            return await Client.GetRangeAsync(prefixKey);
        }

        public static DeleteRangeResponse Delete(string key)
        {
            return Client.Delete(key);
        }

        public static async Task<DeleteRangeResponse> DeleteAsync(string key)
        {
            return await Client.DeleteAsync(key);
        }

        public static DeleteRangeResponse DeleteRange(string prefixKey)
        {
            return Client.DeleteRange(prefixKey);
        }

        public static async Task<DeleteRangeResponse> DeleteRangeAsync(string prefixKey)
        {
            return await Client.DeleteRangeAsync(prefixKey);
        }

        public static void Watch(string key, Action<WatchResponse> method)
        {
            WatchRequest request = new WatchRequest()
            {
                CreateRequest = new WatchCreateRequest()
                {
                    Key = ByteString.CopyFromUtf8(key)
                }
            };
            Client.Watch(request, method);
        }

        public static void WatchKey(string key, Action<WatchResponse> method)
        {
            Client.Watch(key, method);
        }

        public static void Watch(string key, Action<WatchEvent[]> method)
        {
            WatchRequest request = new WatchRequest()
            {
                CreateRequest = new WatchCreateRequest()
                {
                    Key = ByteString.CopyFromUtf8(key)
                }
            };
            Client.Watch(request, method);
        }

        public static MemberAddResponse ClusterAdd(List<string> items)
        {
            MemberAddRequest request = new MemberAddRequest();
            foreach (var item in items)
            {
                request.PeerURLs.Add(item);
            }
            return Client.MemberAdd(request);
        }

        public static async Task<MemberAddResponse> ClusterAddAsync(List<string> items)
        {
            MemberAddRequest request = new MemberAddRequest();
            foreach (var item in items)
            {
                request.PeerURLs.Add(item);
            }
            return await Client.MemberAddAsync(request);
        }

        public static MemberRemoveResponse ClusterDelete(ulong id)
        {
            MemberRemoveRequest request = new MemberRemoveRequest
            {
                ID = id
            };
            return Client.MemberRemove(request);
        }

        public static async Task<MemberRemoveResponse> ClusterDeleteAsync(ulong id)
        {
            MemberRemoveRequest request = new MemberRemoveRequest
            {
                ID = id
            };
            return await Client.MemberRemoveAsync(request);
        }

        public static MemberUpdateResponse ClusterUpdate(ulong id)
        {
            MemberUpdateRequest request = new MemberUpdateRequest
            {
                ID = id
            };
            return Client.MemberUpdate(request);
        }

        public static async Task<MemberUpdateResponse> ClusterUpdateAsync(ulong id)
        {
            MemberUpdateRequest request = new MemberUpdateRequest
            {
                ID = id
            };
            return await Client.MemberUpdateAsync(request);
        }

        public static List<EtcdClusterMember> ClusterList()
        {
            List<EtcdClusterMember> list = new List<EtcdClusterMember>();
            MemberListRequest request = new MemberListRequest();
            MemberListResponse res = Client.MemberList(request);
            foreach(var member in res.Members)
            {
                list.Add(new EtcdClusterMember
                {
                    MemberId = member.ID,
                    MemberName = member.Name,
                    MemberPeerUrls = member.PeerURLs
                });
            }
            return list;
        }

        public static async Task<List<EtcdClusterMember>> ClusterListAsync()
        {
            List<EtcdClusterMember> list = new List<EtcdClusterMember>();
            MemberListRequest request = new MemberListRequest();
            MemberListResponse res = await Client.MemberListAsync(request);
            foreach (var member in res.Members)
            {
                list.Add(new EtcdClusterMember
                {
                    MemberId = member.ID,
                    MemberName = member.Name,
                    MemberPeerUrls = member.PeerURLs
                });
            }
            return list;
        }

        public static TxnResponse TransactionUpdate(string transactionKey)
        {
            var txr = new TxnRequest();
            txr.Success.Add(new RequestOp()
            {
                RequestPut = new PutRequest()
                {
                    Key = Google.Protobuf.ByteString.CopyFrom(transactionKey, System.Text.Encoding.UTF8),
                    Value = Google.Protobuf.ByteString.CopyFrom("", System.Text.Encoding.UTF8)
                }
            });
            return Client.Transaction(txr);
        }
    }
}
