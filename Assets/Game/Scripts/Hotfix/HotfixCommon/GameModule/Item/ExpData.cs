// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     经验数据实体类。
    /// </summary>
    public class ExpData
    {
        /// <summary>
        ///     初始化 <see cref="ExpData" /> 类的新实例。
        /// </summary>
        public ExpData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="ExpData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public ExpData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     获取或设置背包道具基础数据。
        /// </summary>
        public InventoryData Inventory { get; set; }

        /// <summary>
        ///     克隆当前经验数据。
        /// </summary>
        /// <returns>新的 ExpData 实例。</returns>
        public ExpData Clone()
        {
            return new ExpData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置经验数据。
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
            return $"ExpData(Inventory={Inventory})";
        }
    }
}