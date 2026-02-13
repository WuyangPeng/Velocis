using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class LoginResponseHandler : CeleritasHandlerBase<login_response>
    {
        public override void Handle(object sender, header header, login_response message)
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetServerTime(message.CurrentTime);
            userModule.SetUserId(header.ToGateway.UserId);

            Log.Info("login_.CurrentTime ='{0}'.", message.CurrentTime);
        }
    }
}