using Celeritas.Proto.Common;

namespace Game.Scripts.Main.Runtime.Network.PacketHandler
{
    public abstract class CeleritasHandlerBase<T> : ICeleritasHandler
    {
        public abstract void Handle(object sender, header header, T message);
    }
}