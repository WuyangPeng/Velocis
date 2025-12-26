using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class LoginResponseHandler : CeleritasHandlerBase<login_response>
    {
        public override void Handle(object sender, login_response message)
        {
            var procedureMenu = (ProcedureMenu)GameEntry.Procedure.CurrentProcedure;
            if (procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when On Network Connected.");
                return;
            }

            procedureMenu.StartGame();

            Log.Info("login_.CurrentTime ='{0}'.", message.CurrentTime);
        }
    }
}