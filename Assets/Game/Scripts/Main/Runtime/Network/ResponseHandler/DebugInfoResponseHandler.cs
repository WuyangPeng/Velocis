using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Debug;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class DebugInfoResponseHandler : CeleritasHandlerBase<debug_info_response>
    {
        public override void Handle(object sender, header header, debug_info_response message)
        {
            var debugModule = GameEntry.ModuleComponent.GetModule<DebugModule>();
            debugModule.IsDebug = message.OpenDebug;

            GameEntry.Event.Fire(this, DebugInfoEventArgs.Create());
        }
    }
}