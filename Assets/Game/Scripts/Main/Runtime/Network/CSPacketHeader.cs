namespace Game.Scripts.Main.Runtime.Network
{
    public sealed class CSPacketHeader : PacketHeaderBase
    {
        public override PacketType PacketType => PacketType.ClientToServer;
    }
}
