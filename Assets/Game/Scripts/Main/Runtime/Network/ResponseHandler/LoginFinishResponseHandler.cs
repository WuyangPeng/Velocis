using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class LoginFinishResponseHandler : CeleritasHandlerBase<login_finish_response>
    {
        public override void Handle(object sender, header header, login_finish_response message)
        {
            Log.Info("Login Finish.");

            GameEntry.ModuleComponent.LoginFinish();
            
            var procedureMenu = (ProcedureMenu)GameEntry.Procedure.CurrentProcedure;
            if (procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when On Network Connected.");
                return;
            }

            procedureMenu.StartGame();
        }
    }
}