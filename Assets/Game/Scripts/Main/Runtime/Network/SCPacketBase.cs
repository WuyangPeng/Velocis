namespace Game.Scripts.Main.Runtime.Network
{
    public abstract class SCPacketBase : PacketBase
    {
        public override PacketType PacketType => PacketType.ServerToClient;
    }
}
