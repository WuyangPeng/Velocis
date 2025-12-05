using GameFramework;

namespace Game.Scripts.Main.Runtime.RuntimeException
{
    public class GameException :  GameFrameworkException
    {
        public GameException()
        {
        }

 
        public GameException(string message)
            : base(message)
        {
        }

         
    }
}