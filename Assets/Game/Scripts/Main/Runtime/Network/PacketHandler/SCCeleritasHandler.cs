using Celeritas.Proto;
using Game.Scripts.Main.Runtime.Network.Packet;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.PacketHandler
{
    public class SCCeleritasHandler : PacketHandlerBase
    {
        public override int Id => 101;

        public override void Handle(object sender, GameFramework.Network.Packet packet)
        {
            var packetImpl = (SCCeleritas)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());

             if (packetImpl.Common.ToGateway.Code == 1)
            {
                var handler = GameEntry.CeleritasHandler.GetCeleritasHandler<celeritas>();
                if (handler != null)
                {
                    handler.Handle(sender, packetImpl.Celeritas);
                }
                else
                {
                    Log.Error("Can not find handler for 'celeritas'.");
                }
            }
            else
            {
                Log.Info("Receive packet Code ='{0}'.", packetImpl.Common.ToGateway.Code);
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Server.Error"),
                    Message = GameEntry.Localization.GetString("Server.ErrorCode" +
                                                               packetImpl.Common.ToGateway.Code)
                });
            }
        }
    }
}