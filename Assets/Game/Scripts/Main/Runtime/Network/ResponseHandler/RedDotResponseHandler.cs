using System.Collections.Generic;
using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Event;
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

            Dictionary<red_dot_type, int> redDot = new();
            foreach (var element in message.Node)
            {
                redDotModule.AddRedDotNode(new RedDotNode((red_dot_type)element.RedDotType, element.Value));
                if (!message.IsLogin)
                {
                    redDot[(red_dot_type)element.RedDotType] = element.Value;
                }
            }

            if (!message.IsLogin)
            {
                GameEntry.Event.Fire(this, ChangeRedDotEventArgs.Create(redDot));
            }
        }
    }
}