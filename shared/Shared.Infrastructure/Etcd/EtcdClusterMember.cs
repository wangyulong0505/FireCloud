using Google.Protobuf.Collections;

namespace Shared.Infrastructure
{
    public class EtcdClusterMember
    {
        public ulong MemberId { get; set; }

        public string MemberName { get; set; }

        public RepeatedField<string> MemberPeerUrls { get; set; }
    }
}
