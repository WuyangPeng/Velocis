using System;
using ProtoBuf;

namespace Game.Scripts.Main.Runtime.Network.Packet
{
    [Serializable]
    [ProtoContract(Name = @"SCCeleritas")]
    public class SCCeleritas : SCPacketBase
    {
        public override int Id => 101;

        public override void Clear()
        {
        }
    }
}