using Game.Scripts.Main.Runtime.Network.Packet;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Network.PacketHandler
{
    public class SCCeleritasHandler : PacketHandlerBase
    {
        public override int Id => 101;

        public override void Handle(object sender, GameFramework.Network.Packet packet)
        {
            var packetImpl = (SCCeleritas)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
        }
    }
}