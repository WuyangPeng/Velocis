// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     礼包数据实体类。
    /// </summary>
    public class GiftBoxData
    {
        /// <summary>
        ///     初始化 <see cref="GiftBoxData" /> 类的新实例。
        /// </summary>
        public GiftBoxData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="GiftBoxData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public GiftBoxData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     克隆当前礼包数据。
        /// </summary>
        /// <returns>新的 GiftBoxData 实例。</returns>
        public GiftBoxData Clone()
        {
            return new GiftBoxData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置礼包数据。
        /// </summary>
        public void Reset()
        {
            Inventory?.Reset();
        }

        /// <summary>
        ///     返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"GiftBoxData(Inventory={Inventory})";
        }
    }
}