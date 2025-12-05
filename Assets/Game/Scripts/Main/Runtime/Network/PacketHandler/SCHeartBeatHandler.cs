using Game.Scripts.Main.Runtime.Network.Packet;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Network.PacketHandler
{
    public class SCHeartBeatHandler : PacketHandlerBase
    {
        public override int Id => 2;

        public override void Handle(object sender, GameFramework.Network.Packet packet)
        {
            var packetImpl = (SCHeartBeat)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
        }
    }
}
