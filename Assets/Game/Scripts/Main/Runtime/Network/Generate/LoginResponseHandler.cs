using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class LoginResponseHandler : CeleritasHandlerBase<client_login_response>
    {
        public override void Handle(client_login_response message)
        {
            switch (message.PayloadCase)
            {
                case client_login_response.PayloadOneofCase.Login:
                    var login = message.Login;

                    var procedureMenu = (ProcedureMenu)GameEntry.Procedure.CurrentProcedure;
                    if (procedureMenu == null)
                    {
                        Log.Warning("ProcedureMenu is invalid when On Network Connected.");
                        return;
                    }

                    procedureMenu.StartGame();

                    Log.Info("login_.CurrentTime ='{0}'.", login.CurrentTime);
                    break;
            }
        }
    }
}