// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     背包中道具的基础属性类。
    /// </summary>
    public class InventoryData
    {
        /// <summary>
        ///     初始化 <see cref="InventoryData" /> 类的新实例。
        /// </summary>
        public InventoryData()
        {
        }

        /// <summary>
        ///     初始化 <see cref="InventoryData" /> 类的新实例。
        /// </summary>
        /// <param name="itemId">物品唯一ID。</param>
        /// <param name="templateId">物品配置模板ID。</param>
        /// <param name="count">道具堆叠数量。</param>
        /// <param name="position">在背包/格子中的位置索引。</param>
        public InventoryData(long itemId, int templateId, long count, int position)
        {
            ItemId = itemId;
            TemplateId = templateId;
            Count = count;
            Position = position;
        }

        /// <summary>
        ///     获取或设置物品的唯一ID。
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        ///     获取或设置物品的配置模板ID。
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        ///     获取或设置道具堆叠数量。
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        ///     获取或设置道具在格子中的位置。
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        ///     克隆当前道具数据。
        /// </summary>
        /// <returns>新的 InventoryData 实例。</returns>
        public InventoryData Clone()
        {
            return new InventoryData(ItemId, TemplateId, Count, Position);
        }

        /// <summary>
        ///     重置道具基础属性为默认值。
        /// </summary>
        public void Reset()
        {
            ItemId = 0;
            TemplateId = 0;
            Count = 0;
            Position = 0;
        }

        /// <summary>
        ///     返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"InventoryData(ItemId={ItemId}, TemplateId={TemplateId}, Count={Count}, Position={Position})";
        }
    }
}