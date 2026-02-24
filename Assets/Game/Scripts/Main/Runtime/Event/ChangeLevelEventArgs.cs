using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class ChangeLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeLevelEventArgs).GetHashCode();


        public override int Id => EventId;

        public static ChangeLevelEventArgs Create()
        {
            return new ChangeLevelEventArgs();
        }

        public override void Clear()
        {
        }
    }
}