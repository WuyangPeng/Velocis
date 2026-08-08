// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 服务器连接配置信息。
    /// </summary>
    [Serializable]
    public class ConnectionInfo
    {
        /// <summary>服务器的主机地址或 IP 地址。</summary>
        public string host;
        
        /// <summary>服务器的端口号。</summary>
        public int port;
        
        /// <summary>服务器的网络连接类型。</summary>
        public ServerNetworkType server_network;
    }
}
