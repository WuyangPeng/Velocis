using GameFramework.Event;

namespace Game.Scripts.Main.Runtime.Event
{
    public class LoginLoadEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoginLoadEventArgs).GetHashCode();


        public override int Id => EventId;

        public static LoginLoadEventArgs Create()
        {
            return new LoginLoadEventArgs();
        }

        public override void Clear()
        {
        }
    }
}