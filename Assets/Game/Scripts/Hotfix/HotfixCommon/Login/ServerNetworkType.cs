// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 服务器网络连接协议类型。
    /// </summary>
    public enum ServerNetworkType
    {
        /// <summary>标准 TCP 协议连接。</summary>
        Tcp,
        
        /// <summary>标准 HTTP 协议连接。</summary>
        Http,
        
        /// <summary>标准 WebSocket 协议连接。</summary>
        Websocket,
        
        /// <summary>基于 SSL/TLS 加密的 TCP 安全连接。</summary>
        TcpSsl,
        
        /// <summary>基于 HTTPS 的安全连接。</summary>
        Https,
        
        /// <summary>基于 WSS 的安全 WebSocket 连接。</summary>
        WebsocketSecure
    }
}
