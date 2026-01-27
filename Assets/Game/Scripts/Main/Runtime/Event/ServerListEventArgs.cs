using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class ServerListEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ServerListEventArgs).GetHashCode();


        public override int Id => EventId;

        public static ServerListEventArgs Create()
        {
            return new ServerListEventArgs();
        }

        public override void Clear()
        {
        }
    }
}