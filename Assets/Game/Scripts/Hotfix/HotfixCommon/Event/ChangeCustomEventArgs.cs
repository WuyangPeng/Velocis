// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    /// 物品数量改变事件参数。
    /// </summary>
    public class ChangeCustomEventArgs : GameEventArgs
    {
        /// <summary>
        /// 物品数量改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ChangeCustomEventArgs).GetHashCode();

        private ChangeCustomEventArgs(int itemId)
        {
            ItemId = itemId;
        }

        /// <summary>
        /// 获取或设置物品编号。
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 创建物品数量改变事件。
        /// </summary>
        /// <param name="itemId">物品编号。</param>
        /// <returns>创建的事件参数。</returns>
        public static ChangeCustomEventArgs Create(int itemId)
        {
            return new ChangeCustomEventArgs(itemId);
        }

        /// <summary>
        /// 清理物品数量改变事件。
        /// </summary>
        public override void Clear()
        {
            ItemId = 0;
        }
    }
}