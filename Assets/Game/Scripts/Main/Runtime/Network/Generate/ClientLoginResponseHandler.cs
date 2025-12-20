using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class ClientLoginResponseHandler : CeleritasHandlerBase<client_login_response>
    {
        public override void Handle(object sender, client_login_response message)
        {
            switch (message.PayloadCase)
            {
                case client_login_response.PayloadOneofCase.Login:
                    var handler = GameEntry.CeleritasHandler.GetCeleritasHandler<login_response>();
                    if (handler != null)
                    {
                        handler.Handle(sender, message.Login);
                    }
                    else
                    {
                        Log.Error("Can not find handler for 'login_response'.");
                    }

                    break;
            }
        }
    }
}