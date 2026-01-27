using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.RedDot;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class RedDotResponseHandler : CeleritasHandlerBase<red_dot_response>
    {
        public override void Handle(object sender, header header, red_dot_response message)
        {
            var redDotModule = GameEntry.ModuleComponent.GetModule<RedDotModule>();
            if (message.IsLogin)
            {
                redDotModule.ClearRedDotNode();
            }

            foreach (var element in message.Node)
            {
                redDotModule.AddRedDotNode(new RedDotNode((red_dot_type)element.RedDotType, element.Value));
            }
        }
    }
}