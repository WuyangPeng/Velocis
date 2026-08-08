// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     登录加载事件参数。
    /// </summary>
    public class LoginLoadEventArgs : GameEventArgs
    {
        /// <summary>
        ///     登录加载事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LoginLoadEventArgs).GetHashCode();

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     创建登录加载事件。
        /// </summary>
        /// <returns>创建的事件参数。</returns>
        public static LoginLoadEventArgs Create()
        {
            return new LoginLoadEventArgs();
        }

        /// <summary>
        ///     清理登录加载事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}