// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Proto.Common;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     背包道具网络数据转换的扩展方法类。
    /// </summary>
    public static class InventoryDataExtensions
    {
        /// <summary>
        ///     将网络协议传输的 <see cref="inventory_data" /> 转换为客户端实体 <see cref="InventoryData" />。
        /// </summary>
        /// <param name="data">协议层数据。</param>
        /// <returns>客户端道具实体，若传入的协议数据为 null 则返回 null。</returns>
        public static InventoryData ToInventoryData(this inventory_data data)
        {
            var inventoryData = new InventoryData
            {
                ItemId = data.ItemId,
                TemplateId = data.TemplateId,
                Count = data.Count,
                Position = data.Position
            };

            return inventoryData;
        }
    }
}