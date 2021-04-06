using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.Collections;

namespace Shared.Infrastructure.Etcd
{
    public class EtcdClusterMember
    {
        public ulong MemberId { get; set; }

        public string MemberName { get; set; }

        public RepeatedField<string> MemberPeerUrls { get; set; }
    }
}
