using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Role;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class RoleResponseHandler : CeleritasHandlerBase<role_response>
    {
        public override void Handle(object sender, header header, role_response message)
        {
            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();

            roleModule.SetRole(message);
        }
    }
}