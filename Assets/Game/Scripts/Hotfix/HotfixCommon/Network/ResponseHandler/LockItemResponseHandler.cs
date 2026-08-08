// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     道具锁定/解锁状态变更响应处理器。
    /// </summary>
    public class LockItemResponseHandler : CeleritasHandlerBase<lock_item_response>
    {
        protected override void Handle(object sender, header header, lock_item_response message)
        {
        }
    }
}