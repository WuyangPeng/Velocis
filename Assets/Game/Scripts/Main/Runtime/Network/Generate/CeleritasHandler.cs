using Celeritas.Proto;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class CeleritasHandler : CeleritasHandlerBase<celeritas>
    {
        public override void Handle(object sender, celeritas message)
        {
            switch (message.PayloadCase)
            {
                case celeritas.PayloadOneofCase.CeleritasResponse:
                    var handler = GameEntry.CeleritasHandler.GetCeleritasHandler<response>();
                    if (handler != null)
                    {
                        handler.Handle(sender, message.CeleritasResponse);
                    }
                    else
                    {
                        Log.Error("Can not find handler for 'response'.");
                    }

                    break;
            }
        }
    }
}