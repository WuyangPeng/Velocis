// 创建时间：2026-07-06
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network.Packet;
using GameFramework;

namespace Game.Scripts.Hotfix.HotfixCommon.Network
{
    /// <summary>
    ///     协议助手类，用于生成和初始化协议包及处理 RPC 编号。
    /// </summary>
    public class ProtoHelper
    {
        private static int _rpc;

        /// <summary>
        ///     获取递增的 RPC 序列号。
        /// </summary>
        /// <returns>新的 RPC 序列号</returns>
        public static int GetRpc()
        {
            return ++_rpc;
        }

        /// <summary>
        ///     从引用池中获取并初始化一个 <see cref="CSCeleritas" /> 协议包，并自动分配 RPC 序列号。
        /// </summary>
        /// <returns>初始化后的协议包</returns>
        public static CSCeleritas GetProto()
        {
            var packet = ReferencePool.Acquire<CSCeleritas>();
            packet.Common.Client = new client_message_header
            {
                Rpc = GetRpc()
            };

            return packet;
        }
    }
}