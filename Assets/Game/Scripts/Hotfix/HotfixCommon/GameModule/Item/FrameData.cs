// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     头像框数据实体类。
    /// </summary>
    public class FrameData
    {
        /// <summary>
        ///     初始化 <see cref="FrameData" /> 类的新实例。
        /// </summary>
        public FrameData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="FrameData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public FrameData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     获取或设置背包道具基础数据。
        /// </summary>
        public InventoryData Inventory { get; set; }

        /// <summary>
        ///     克隆当前头像框数据。
        /// </summary>
        /// <returns>新的 FrameData 实例。</returns>
        public FrameData Clone()
        {
            return new FrameData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置头像框数据。
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
            return $"FrameData(Inventory={Inventory})";
        }
    }
}