// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     资源类道具数据实体类。
    /// </summary>
    public class ResourceData
    {
        /// <summary>
        ///     初始化 <see cref="ResourceData" /> 类的新实例。
        /// </summary>
        public ResourceData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="ResourceData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public ResourceData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     获取或设置背包道具基础数据。
        /// </summary>
        public InventoryData Inventory { get; set; }

        /// <summary>
        ///     克隆当前资源数据。
        /// </summary>
        /// <returns>新的 ResourceData 实例。</returns>
        public ResourceData Clone()
        {
            return new ResourceData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置资源数据。
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
            return $"ResourceData(Inventory={Inventory})";
        }
    }
}