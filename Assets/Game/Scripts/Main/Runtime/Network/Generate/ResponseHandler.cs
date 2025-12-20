using Celeritas.Proto;
using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class ResponseHandler : CeleritasHandlerBase<response>
    {
        public override void Handle(object sender, response message)
        {
            switch (message.PayloadCase)
            {
                case response.PayloadOneofCase.Client:
                    var handler = GameEntry.CeleritasHandler.GetCeleritasHandler<client_response>();
                    if (handler != null)
                    {
                        handler.Handle(sender, message.Client);
                    }
                    else
                    {
                        Log.Error("Can not find handler for 'client_response'.");
                    }

                    break;
            }
        }
    }
}