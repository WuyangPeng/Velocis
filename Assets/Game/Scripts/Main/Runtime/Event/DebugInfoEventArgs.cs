using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class DebugInfoEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(DebugInfoEventArgs).GetHashCode();


        public override int Id => EventId;

        public static DebugInfoEventArgs Create()
        {
            return new DebugInfoEventArgs();
        }

        public override void Clear()
        {
        }
    }
}