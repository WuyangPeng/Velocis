// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     调试信息事件参数。
    /// </summary>
    public class DebugInfoEventArgs : GameEventArgs
    {
        /// <summary>
        ///     调试信息事件编号。
        /// </summary>
        public static readonly int EventId = typeof(DebugInfoEventArgs).GetHashCode();

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     创建调试信息事件。
        /// </summary>
        /// <returns>创建的事件参数。</returns>
        public static DebugInfoEventArgs Create()
        {
            return new DebugInfoEventArgs();
        }

        /// <summary>
        ///     清理调试信息事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}