// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Game.Scripts.Hotfix.HotfixCommon.Game;

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 获取认证 Token 的 HTTP 响应体数据结构。
    /// </summary>
    [System.Serializable]
    public class TokenHttpResponse
    {
        /// <summary>响应的错误码类型，默认值为 <see cref="GameErrorType.Unknown"/>。</summary>
        public GameErrorType code = GameErrorType.Unknown;
        
        /// <summary>响应的附带文本消息（通常是错误说明）。</summary>
        public string message;
        
        /// <summary>认证成功后返回的安全令牌 Token 字符串。</summary>
        public string token;
        
        /// <summary>Token 过期的毫秒时间戳。</summary>
        public long expire_milliseconds;
    }
}