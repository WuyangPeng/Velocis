using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class LoginFinishResponseHandler : CeleritasHandlerBase<login_finish_response>
    {
        public override void Handle(object sender, login_finish_response message)
        {
            GameEntry.ModuleComponent.LoginFinish();
        }
    }
}