// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     建筑数据实体类。
    /// </summary>
    public class BuildingData
    {
        /// <summary>
        ///     初始化 <see cref="BuildingData" /> 类的新实例。
        /// </summary>
        public BuildingData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="BuildingData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public BuildingData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="BuildingData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        /// <param name="level">建筑等级。</param>
        public BuildingData(InventoryData inventory, int level)
        {
            Inventory = inventory ?? new InventoryData();
            Level = level;
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     获取或设置建筑等级。
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     克隆当前建筑数据。
        /// </summary>
        /// <returns>新的 BuildingData 实例。</returns>
        public BuildingData Clone()
        {
            return new BuildingData(Inventory?.Clone(), Level);
        }

        /// <summary>
        ///     重置建筑数据。
        /// </summary>
        public void Reset()
        {
            Inventory?.Reset();
            Level = 0;
        }

        /// <summary>
        ///     返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"BuildingData(Inventory={Inventory}, Level={Level})";
        }
    }
}