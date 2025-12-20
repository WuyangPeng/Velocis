using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.Packet;
using GameFramework;

namespace Game.Scripts.Main.Runtime.Network
{
    public class ProtoHelper
    {
        private static int rpc;

        public static int GetRpc()
        {
            return ++rpc;
        }

        public static CSCeleritas GetProto()
        {
            var packet = ReferencePool.Acquire<CSCeleritas>();
            packet.Common.Client = new client_message_header
            {
                Rpc = GetRpc()
            };

            return packet;
        }
    }
}