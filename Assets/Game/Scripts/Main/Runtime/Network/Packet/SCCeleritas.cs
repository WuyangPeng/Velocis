using System;
using Celeritas.Proto;
using Celeritas.Proto.Common;
using ProtoBuf;

namespace Game.Scripts.Main.Runtime.Network.Packet
{
    [Serializable]
    [ProtoContract(Name = @"SCCeleritas")]
    public class SCCeleritas : SCPacketBase
    {
        public header Common { get; set; }
        public celeritas Celeritas { get; set; }
        public override int Id => 101;

        public override void Clear()
        {
        }
    }
}