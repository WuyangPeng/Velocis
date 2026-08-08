// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     关闭服务器列表事件参数。
    /// </summary>
    public class CloseServerListEventArgs : GameEventArgs
    {
        /// <summary>
        ///     关闭服务器列表事件编号。
        /// </summary>
        public static readonly int EventId = typeof(CloseServerListEventArgs).GetHashCode();

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     创建关闭服务器列表事件。
        /// </summary>
        /// <returns>创建的事件参数。</returns>
        public static CloseServerListEventArgs Create()
        {
            return new CloseServerListEventArgs();
        }

        /// <summary>
        ///     清理关闭服务器列表事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}