// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using System.Collections.Generic;
using Game.Scripts.Hotfix.HotfixCommon.Game;

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 获取登录服务器列表的 HTTP 响应体数据结构。
    /// </summary>
    [Serializable]
    public class LoginServersResponse
    {
        /// <summary>响应的附带文本消息（通常是错误说明）。</summary>
        public string message;
        
        /// <summary>可用的登录服务器详细信息列表。</summary>
        public List<LoginServerInfo> login_server_info;
        
        /// <summary>大区/分区信息列表。</summary>
        public List<string> zones;
        
        /// <summary>响应的错误码类型，默认值为 <see cref="GameErrorType.Unknown"/>。</summary>
        public GameErrorType code = GameErrorType.Unknown;
    }
}