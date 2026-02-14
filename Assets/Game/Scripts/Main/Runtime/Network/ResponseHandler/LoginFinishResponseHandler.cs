using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
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

            GameEntry.Event.Fire(this, LoginProgressEventArgs.Create(1f));
        }
    }
}