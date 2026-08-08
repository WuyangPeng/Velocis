// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     武将道具数据实体类。
    /// </summary>
    public class HeroData
    {
        /// <summary>
        ///     初始化 <see cref="HeroData" /> 类的新实例。
        /// </summary>
        public HeroData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="HeroData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public HeroData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     克隆当前武将数据。
        /// </summary>
        /// <returns>新的 HeroData 实例。</returns>
        public HeroData Clone()
        {
            return new HeroData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置武将数据。
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
            return $"HeroData(Inventory={Inventory})";
        }
    }
}