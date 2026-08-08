// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using UnityGameFramework.Runtime;

namespace Game.Scripts.Hotfix.HotfixCommon.Network
{
    /// <summary>
    ///     网络组件扩展方法。
    /// </summary>
    public static class NetworkExtensions
    {
        /// <summary>
        ///     使用指定网络通道发送协议包。
        /// </summary>
        /// <param name="networkComponent">网络组件。</param>
        /// <param name="channelName">通道名称。</param>
        /// <param name="packet">协议包。</param>
        public static void Send(this NetworkComponent networkComponent, string channelName, GameFramework.Network.Packet packet)
        {
            if (networkComponent == null)
            {
                Log.Error("NetworkComponent is null.");
                return;
            }

            var channel = networkComponent.GetNetworkChannel(channelName);
            if (channel == null)
            {
                Log.Error($"Channel '{channelName}' not found.");
                return;
            }

            channel.Send(packet);
        }
    }
}