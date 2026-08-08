namespace Game.Scripts.Main.Runtime.Network.PacketHandler
{
    public interface ICeleritasHandler
    {
    }

    public interface ICeleritasHandler<T> : ICeleritasHandler
    {
        void Handle(object sender, object header, T message);
    }
}