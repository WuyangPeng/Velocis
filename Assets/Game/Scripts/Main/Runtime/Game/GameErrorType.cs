namespace Game.Scripts.Main.Runtime.Game
{
    public enum GameErrorType
    {
        Unknown = 0,
        Success = 1,

        // 通用错误
        InvalidParameter = 100,
        TimestampExpired = 101,
        SignError = 102,
        SentTooFrequently = 103,
        CodeExpired = 104,
        CodeError = 105,
        TokenError = 106,
        PasswordError = 107,
        SdkError = 108,
        ServerError = 109,

        // auth错误
        AccountError = 1000,
        NoGuestAccount = 1001,
        AccountBound = 1002,

        // 数据库错误
        RedisError = 2000,
        MysqlError = 2001,
    }
}