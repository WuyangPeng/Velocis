// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     装备数据实体类。
    /// </summary>
    public class EquipmentData
    {
        /// <summary>
        ///     初始化 <see cref="EquipmentData" /> 类的新实例。
        /// </summary>
        public EquipmentData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="EquipmentData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public EquipmentData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="EquipmentData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        /// <param name="strength">强化等级。</param>
        /// <param name="durability">当前耐久度。</param>
        public EquipmentData(InventoryData inventory, int strength, int durability)
        {
            Inventory = inventory ?? new InventoryData();
            Strength = strength;
            Durability = durability;
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     获取或设置装备的强化等级。
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        ///     获取或设置装备的耐久度。
        /// </summary>
        public int Durability { get; set; }

        /// <summary>
        ///     克隆当前装备数据。
        /// </summary>
        /// <returns>新的 EquipmentData 实例。</returns>
        public EquipmentData Clone()
        {
            return new EquipmentData(Inventory?.Clone(), Strength, Durability);
        }

        /// <summary>
        ///     重置装备数据。
        /// </summary>
        public void Reset()
        {
            Inventory?.Reset();
            Strength = 0;
            Durability = 0;
        }

        /// <summary>
        ///     返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"EquipmentData(Inventory={Inventory}, Strength={Strength}, Durability={Durability})";
        }
    }
}