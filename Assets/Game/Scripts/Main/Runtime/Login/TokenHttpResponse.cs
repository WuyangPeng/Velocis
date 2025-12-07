using Game.Scripts.Main.Runtime.Game;

namespace Game.Scripts.Main.Runtime.Login
{
    [System.Serializable]
    public class TokenHttpResponse
    {
        public GameErrorType code = GameErrorType.Unknown;
        public string message;
        public string token;
        public long expire_milliseconds;
    }
}