using System.Collections.Generic;
using Celeritas.Config;
using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class ChangeRedDotEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeRedDotEventArgs).GetHashCode();

        public ChangeRedDotEventArgs(Dictionary<red_dot_type, int> redDot)
        {
            RedDot = redDot;
        }


        public override int Id => EventId;

        public Dictionary<red_dot_type, int> RedDot { get; }


        public static ChangeRedDotEventArgs Create(Dictionary<red_dot_type, int> redDot)
        {
            return new ChangeRedDotEventArgs(redDot);
        }

        public override void Clear()
        {
            RedDot.Clear();
        }
    }
}