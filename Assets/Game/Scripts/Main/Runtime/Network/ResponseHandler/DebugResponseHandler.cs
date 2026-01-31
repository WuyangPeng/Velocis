using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class DebugResponseHandler : CeleritasHandlerBase<debug_response>
    {
        public override void Handle(object sender, header header, debug_response message)
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Button.Debug"),
                Message = GameEntry.Localization.GetString("Home.OperationSuccessful")
            });
        }
    }
}