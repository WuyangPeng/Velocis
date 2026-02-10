using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class NetworkEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NetworkEventArgs).GetHashCode();


        public override int Id => EventId;

        public static NetworkEventArgs Create()
        {
            return new NetworkEventArgs();
        }

        public override void Clear()
        {
        }
    }
}