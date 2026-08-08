// 创建时间：2026-08-01
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Game.Scripts.Main.Runtime.Network;
using Game.Scripts.Main.Runtime.Network.Packet;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.Model
{
    /// <summary>
    /// 消息包验证结果。
    /// </summary>
    public class PacketValidationResult
    {
        /// <summary>
        /// 验证是否成功。
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// 消息包基类实现。
        /// </summary>
        public PacketBase PacketImpl { get; }

        /// <summary>
        /// 具体的 CSCeleritas 协议数据。
        /// </summary>
        public CSCeleritas Celeritas { get; }

        private PacketValidationResult(bool isValid, PacketBase packetImpl, CSCeleritas celeritas)
        {
            IsValid = isValid;
            PacketImpl = packetImpl;
            Celeritas = celeritas;
        }

        /// <summary>
        /// 创建验证成功的包验证结果。
        /// </summary>
        public static PacketValidationResult Success(PacketBase packetImpl, CSCeleritas celeritas)
        {
            return new PacketValidationResult(true, packetImpl, celeritas);
        }

        /// <summary>
        /// 创建验证失败的包验证结果。
        /// </summary>
        public static PacketValidationResult Fail()
        {
            return new PacketValidationResult(false, null, null);
        }
    }
}
