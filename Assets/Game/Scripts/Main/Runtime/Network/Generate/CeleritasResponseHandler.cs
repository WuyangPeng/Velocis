using Celeritas.Proto;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class CeleritasResponseHandler : CeleritasHandlerBase<response>
    {
        public override void Handle(response message)
        {
            switch (message.PayloadCase)
            {
                case response.PayloadOneofCase.Client:
                    new ClientResponseHandler().Handle(message.Client);
                    break;
            }
        }
    }
}