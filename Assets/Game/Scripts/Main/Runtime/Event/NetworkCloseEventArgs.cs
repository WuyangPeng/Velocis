using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class NetworkCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NetworkCloseEventArgs).GetHashCode();


        public override int Id => EventId;

        public static NetworkCloseEventArgs Create()
        {
            return new NetworkCloseEventArgs();
        }

        public override void Clear()
        {
        }
    }
}