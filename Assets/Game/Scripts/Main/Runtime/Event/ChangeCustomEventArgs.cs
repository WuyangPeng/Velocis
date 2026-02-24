using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class ChangeCustomEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeCustomEventArgs).GetHashCode();

        public ChangeCustomEventArgs(int itemId)
        {
            ItemId = itemId;
        }

        public int ItemId { get; set; }

        public override int Id => EventId;

        public static ChangeCustomEventArgs Create(int itemId)
        {
            return new ChangeCustomEventArgs(itemId);
        }

        public override void Clear()
        {
            ItemId = 0;
        }
    }
}