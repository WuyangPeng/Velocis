using System;
using ProtoBuf;

namespace Game.Scripts.Main.Runtime.Network.Packet
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public class SCHeartBeat : SCPacketBase
    {
        public SCHeartBeat()
        {
        }

        public override int Id => 2;

        public override void Clear()
        {
        }
    }
}
