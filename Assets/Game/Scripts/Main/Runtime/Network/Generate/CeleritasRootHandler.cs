using Celeritas.Proto;

namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public class CeleritasRootHandler : CeleritasHandlerBase<celeritas>
    {
        public override void Handle(celeritas message)
        {
            switch (message.PayloadCase)
            {
                case celeritas.PayloadOneofCase.CeleritasResponse:
                    new CeleritasResponseHandler().Handle(message.CeleritasResponse);
                    break;
            }
        }
    }
}