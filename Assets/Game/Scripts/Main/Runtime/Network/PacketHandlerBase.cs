using GameFramework.Network;

namespace Game.Scripts.Main.Runtime.Network
{
    public abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract int Id
        {
            get;
        }

        public abstract void Handle(object sender, GameFramework.Network.Packet packet);
    }
}
