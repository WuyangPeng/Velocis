using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class LoginFinishResponseHandler : CeleritasHandlerBase<login_finish_response>
    {
        public override void Handle(object sender, login_finish_response message)
        {
            Log.Info("Login Finish.");

            GameEntry.ModuleComponent.LoginFinish();
        }
    }
}