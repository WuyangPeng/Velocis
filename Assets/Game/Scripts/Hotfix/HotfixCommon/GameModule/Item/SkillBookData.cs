// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    /// 技能书数据实体类。
    /// </summary>
    public class SkillBookData
    {
        /// <summary>
        ///     初始化 <see cref="SkillBookData" /> 类的新实例。
        /// </summary>
        public SkillBookData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="SkillBookData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public SkillBookData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     克隆当前技能书数据。
        /// </summary>
        /// <returns>新的 SkillBookData 实例。</returns>
        public SkillBookData Clone()
        {
            return new SkillBookData(Inventory?.Clone());
        }

        /// <summary>
        ///     重置技能书数据。
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
            return $"SkillBookData(Inventory={Inventory})";
        }
    }
}