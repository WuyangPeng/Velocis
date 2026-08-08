// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    /// 士兵数据实体类。
    /// </summary>
    public class SoldierData
    {
        /// <summary>
        ///     初始化 <see cref="SoldierData" /> 类的新实例。
        /// </summary>
        public SoldierData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="SoldierData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public SoldierData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     克隆当前士兵数据。
        /// </summary>
        /// <returns>新的 SoldierData 实例。</returns>
        public SoldierData Clone()
        {
            return new SoldierData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置士兵数据。
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
            return $"SoldierData(Inventory={Inventory})";
        }
    }
}