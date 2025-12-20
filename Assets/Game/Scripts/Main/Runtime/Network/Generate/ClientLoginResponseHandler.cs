using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class ClientLoginResponseHandler : CeleritasHandlerBase<client_login_response>
    {
        public override void Handle(object sender, client_login_response message)
        {
            var networkChannelHelper = (NetworkChannelHelper)sender;
            switch (message.PayloadCase)
            {
                case client_login_response.PayloadOneofCase.Login:
                    var handler = networkChannelHelper.GetCeleritasHandler<login_response>();
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