using Celeritas.Proto.Client;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class PlayerResponseHandler : CeleritasHandlerBase<client_player_response>
    {
        public override void Handle(client_player_response message)
        {
            switch (message.PayloadCase)
            {
                case client_player_response.PayloadOneofCase.Login:
                    new LoginResponseHandler().Handle(message.Login);
                    break;
            }
        }
    }
}