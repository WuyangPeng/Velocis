using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public sealed class ChangeNameEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeNameEventArgs).GetHashCode();


        public override int Id => EventId;

        public static ChangeNameEventArgs Create()
        {
            return new ChangeNameEventArgs();
        }

        public override void Clear()
        {
        }
    }
}