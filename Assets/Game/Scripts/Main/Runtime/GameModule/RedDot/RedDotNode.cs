using Celeritas.Config;

namespace Game.Scripts.Main.Runtime.GameModule.RedDot
{
    public class RedDotNode
    {
        public RedDotNode(red_dot_type type, int value)
        {
            Type = type;
            Value = value;
        }

        public red_dot_type Type { get; }

        public int Value { get; }
    }
}