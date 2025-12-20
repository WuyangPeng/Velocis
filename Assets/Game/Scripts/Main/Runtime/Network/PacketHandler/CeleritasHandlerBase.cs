namespace Game.Scripts.Main.Runtime.Network.Generate
{
    public abstract class CeleritasHandlerBase<T>
    {
        public abstract void Handle(T message);
    }
}