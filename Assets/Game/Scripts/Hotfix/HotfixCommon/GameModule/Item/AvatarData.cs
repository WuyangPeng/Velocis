// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     头像数据实体类。
    /// </summary>
    public class AvatarData
    {
        /// <summary>
        ///     初始化 <see cref="AvatarData" /> 类的新实例。
        /// </summary>
        public AvatarData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="AvatarData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public AvatarData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     获取或设置背包道具基础数据。
        /// </summary>
        public InventoryData Inventory { get; set; }

        /// <summary>
        ///     克隆当前头像数据。
        /// </summary>
        /// <returns>新的 AvatarData 实例。</returns>
        public AvatarData Clone()
        {
            return new AvatarData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置头像数据。
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
            return $"AvatarData(Inventory={Inventory})";
        }
    }
}