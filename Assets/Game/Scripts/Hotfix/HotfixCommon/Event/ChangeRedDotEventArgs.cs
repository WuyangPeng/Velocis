// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Celeritas.Config;
using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    ///     红点状态改变事件参数。
    /// </summary>
    public class ChangeRedDotEventArgs : GameEventArgs
    {
        /// <summary>
        ///     红点状态改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ChangeRedDotEventArgs).GetHashCode();

        private ChangeRedDotEventArgs(Dictionary<red_dot_type, int> redDot)
        {
            RedDot = redDot;
        }

        /// <summary>
        ///     获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        ///     获取红点数据字典。
        /// </summary>
        public Dictionary<red_dot_type, int> RedDot { get; }

        /// <summary>
        ///     创建红点状态改变事件。
        /// </summary>
        /// <param name="redDot">红点数据字典。</param>
        /// <returns>创建的事件参数。</returns>
        public static ChangeRedDotEventArgs Create(Dictionary<red_dot_type, int> redDot)
        {
            return new ChangeRedDotEventArgs(redDot);
        }

        /// <summary>
        ///     清理红点状态改变事件。
        /// </summary>
        public override void Clear()
        {
            RedDot.Clear();
        }
    }
}