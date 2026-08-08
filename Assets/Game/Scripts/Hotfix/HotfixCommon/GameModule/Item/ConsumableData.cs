// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Item
{
    /// <summary>
    ///     消耗品数据实体类。
    /// </summary>
    public class ConsumableData
    {
        /// <summary>
        ///     初始化 <see cref="ConsumableData" /> 类的新实例。
        /// </summary>
        public ConsumableData()
        {
            Inventory = new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="ConsumableData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        public ConsumableData(InventoryData inventory)
        {
            Inventory = inventory ?? new InventoryData();
        }

        /// <summary>
        ///     初始化 <see cref="ConsumableData" /> 类的新实例。
        /// </summary>
        /// <param name="inventory">背包道具基础数据。</param>
        /// <param name="expireTime">过期时间戳。</param>
        public ConsumableData(InventoryData inventory, long expireTime)
        {
            Inventory = inventory ?? new InventoryData();
            ExpireTime = expireTime;
        }

        private InventoryData Inventory { get; }

        /// <summary>
        ///     获取或设置消耗品过期时间戳。
        /// </summary>
        public long ExpireTime { get; set; }

        /// <summary>
        ///     克隆当前消耗品数据。
        /// </summary>
        /// <returns>新的 ConsumableData 实例。</returns>
        public ConsumableData Clone()
        {
            return new ConsumableData(Inventory?.Clone(), ExpireTime);
        }

        /// <summary>
        ///     重置消耗品数据。
        /// </summary>
        public void Reset()
        {
            Inventory?.Reset();
            ExpireTime = 0;
        }

        /// <summary>
        ///     返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"ConsumableData(Inventory={Inventory}, ExpireTime={ExpireTime})";
        }
    }
}