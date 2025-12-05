using System;
using ProtoBuf;

namespace Game.Scripts.Main.Runtime.Network.Packet
{
    [Serializable, ProtoContract(Name = @"CSHeartBeat")]
    public class CSHeartBeat : CSPacketBase
    {
        public CSHeartBeat()
        {
        }

        public override int Id => 1;

        public override void Clear()
        {
        }
    }
}
