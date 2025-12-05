namespace Game.Scripts.Main.Runtime.Network
{
    public sealed class SCPacketHeader : PacketHeaderBase
    {
        public override PacketType PacketType => PacketType.ServerToClient;
    }
}
