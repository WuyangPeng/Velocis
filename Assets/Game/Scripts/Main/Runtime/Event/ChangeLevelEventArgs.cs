using Celeritas.Config;
using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class ChangeLevelEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeLevelEventArgs).GetHashCode();

        public ChangeLevelEventArgs(develop_system_type systemType)
        {
            SystemType = systemType;
        }

        public develop_system_type SystemType { get; private set; }

        public override int Id => EventId;

        public static ChangeLevelEventArgs Create(develop_system_type systemType)
        {
            return new ChangeLevelEventArgs(systemType);
        }

        public override void Clear()
        {
            SystemType = develop_system_type.none;
        }
    }
}