// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using Celeritas.Proto;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.Network;
using ProtoBuf;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.Packet
{
    /// <summary>
    /// 服务端 → 客户端的 Celeritas 数据包（SC = Server to Client）。
    /// 包含公共消息头 <see cref="header"/> 和具体的 <see cref="celeritas"/> 消息体。
    /// </summary>
    [Serializable]
    [ProtoContract(Name = @"SCCeleritas")]
    public class SCCeleritas : SCPacketBase
    {
        public header Common { get; set; }
        public celeritas Celeritas { get; set; }
        public override int Id => 101;

        public override void Clear()
        {
        }
    }
}