using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPSR_FDPSLib.Blobs;

public class UserUnion : BlobType
{
    public long? UnionId;
    public ulong? NextJoinTime;
    public Dictionary<long, long>? ReqUnionTimes;
    public bool? JoinFlag;
    public List<long>? CollectedIds;
    public long? ActiveAwardResetTime;
    public List<int>? ReceivedAwardIds;
    //...

    public UserUnion()
    {
    }

    public UserUnion(BlobReader blob) : base(ref blob)
    {
    }

    public override bool ParseField(int index, ref BlobReader blob)
    {
        switch (index)
        {
            case Zproto.UserUnion.UnionIdFieldNumber:
                UnionId = blob.ReadLong();
                return true;
            case Zproto.UserUnion.NextJoinTimeFieldNumber:
                NextJoinTime = blob.ReadULong();
                return true;
            case Zproto.UserUnion.ReqUnionTimesFieldNumber:
                ReqUnionTimes = blob.ReadHashMap<long, long>();
                return true;
            case Zproto.UserUnion.CollectedIdsFieldNumber:
                CollectedIds = blob.ReadList<long>();
                return true;
            case Zproto.UserUnion.ActiveAwardResetTimeFieldNumber:
                ActiveAwardResetTime = blob.ReadLong();
                return true;
            case Zproto.UserUnion.ReceivedAwardIdsFieldNumber:
                ReceivedAwardIds = blob.ReadList<int>();
                return true;
            default:
                return false;
        }
    }
}
