using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.PacketHandler;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class ClientResponseHandler : CeleritasHandlerBase<client_response>
    {
        public override void Handle(client_response message)
        {
            switch (message.PayloadCase)
            {
                case client_response.PayloadOneofCase.Player:
                    new PlayerResponseHandler().Handle(message.Player);
                    break;
            }
        }
    }
}