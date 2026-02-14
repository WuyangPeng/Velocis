using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class LoginProgressEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoginProgressEventArgs).GetHashCode();

        public override int Id => EventId;

        public float Progress { get; private set; }

        public static LoginProgressEventArgs Create(float progress)
        {
            var eventArgs = new LoginProgressEventArgs();
            eventArgs.Progress = progress;
            return eventArgs;
        }

        public override void Clear()
        {
            Progress = 0f;
        }
    }
}
