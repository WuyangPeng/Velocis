// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Config;
using Celeritas.Proto.Client;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     背包道具管理模块基类，处理物品的选择、激活和加锁解锁的协议发送。
    /// </summary>
    public class ItemModule : BaseModule
    {
        /// <summary>
        ///     发送道具选择/装配的消息到服务器。
        /// </summary>
        /// <param name="itemType">物品大类类型。</param>
        /// <param name="childType">子选择类型。</param>
        /// <param name="operationId">操作类型ID。</param>
        /// <param name="parameter">参数值。</param>
        /// <param name="selectedId">选中的物品实例ID。</param>
        public void SendItemSelectedMessage(item_type itemType, item_selected_child_type childType, long operationId, int parameter, long selectedId)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientItem_ItemSelected();
            request.ItemSelected = new item_selected_data
            {
                ItemType = (int)itemType,
                ChildType = (int)childType,
                OperationId = (int)operationId,
                Parameter = parameter,
                SelectedId = selectedId
            };

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送激活/使用道具的消息协议到服务器。
        /// </summary>
        /// <param name="itemId">物品配置ID。</param>
        public void SendActivateItemMessage(int itemId)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientItem_ActivateItem();
            request.TemplateId = itemId;

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送锁定或解锁道具的协议消息到服务器。
        /// </summary>
        /// <param name="itemId">道具唯一ID。</param>
        /// <param name="isLocked">是否锁死/锁定道具。</param>
        public void SendActivateLockItem(int itemId, bool isLocked)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientItem_LockItem();
            request.ItemId = itemId;
            request.IsLocked = isLocked;

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }
    }
}