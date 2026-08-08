// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     名字改变事件参数。
    /// </summary>
    public sealed class ChangeNameEventArgs : GameEventArgs
    {
        /// <summary>
        ///     名字改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ChangeNameEventArgs).GetHashCode();

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     创建名字改变事件。
        /// </summary>
        /// <returns>创建的事件参数。</returns>
        public static ChangeNameEventArgs Create()
        {
            return new ChangeNameEventArgs();
        }

        /// <summary>
        ///     清理名字改变事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}