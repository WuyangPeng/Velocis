namespace Game.Scripts.Main.Runtime.Network
{
    public abstract class CSPacketBase : PacketBase
    {
        public override PacketType PacketType => PacketType.ClientToServer;
    }
}
