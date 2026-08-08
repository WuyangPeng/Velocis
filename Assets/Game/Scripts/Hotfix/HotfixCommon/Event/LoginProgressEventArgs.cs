// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     登录进度事件参数。
    /// </summary>
    public class LoginProgressEventArgs : GameEventArgs
    {
        /// <summary>
        ///     登录进度事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoginProgressEventArgs).GetHashCode();

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     获取登录进度。
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        ///     创建登录进度事件。
        /// </summary>
        /// <param name="progress">登录进度值。</param>
        /// <returns>创建的事件参数。</returns>
        public static LoginProgressEventArgs Create(float progress)
        {
            var eventArgs = new LoginProgressEventArgs
            {
                Progress = progress
            };
            return eventArgs;
        }

        /// <summary>
        ///     清理登录进度事件。
        /// </summary>
        public override void Clear()
        {
            Progress = 0f;
        }
    }
}