// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.User
{
    /// <summary>
    ///     账户登录凭证 Token 类。
    /// </summary>
    public class Token
    {
        private long _expireMilliseconds;

        private string _token = "";

        /// <summary>
        ///     设置 Token 及其过期时间。
        /// </summary>
        /// <param name="token">Token 字符串。</param>
        /// <param name="expireMilliseconds">过期毫秒时间戳。</param>
        public void SetToken(string token, long expireMilliseconds)
        {
            _token = token;
            _expireMilliseconds = expireMilliseconds;
        }

        /// <summary>
        ///     获取 Token 字符串。
        /// </summary>
        /// <returns>Token 字符串。</returns>
        public string GetToken()
        {
            return _token;
        }
    }
}