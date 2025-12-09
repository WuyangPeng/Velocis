namespace Game.Scripts.Main.Runtime.Account
{
    public class Token
    {
        private long _expireMilliseconds;

        private string _token = "";

        public void SetToken(string token, long expireMilliseconds)
        {
            _token = token;
            _expireMilliseconds = expireMilliseconds;
        }

        public string GetToken()
        {
            return _token;
        }
    }
}