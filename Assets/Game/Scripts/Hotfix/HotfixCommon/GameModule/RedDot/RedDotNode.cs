// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Config;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.RedDot
{
    /// <summary>
    ///     红点数据节点类。
    /// </summary>
    public class RedDotNode
    {
        /// <summary>
        ///     初始化 <see cref="RedDotNode" /> 类的新实例。
        /// </summary>
        /// <param name="type">红点类型。</param>
        /// <param name="value">红点状态计数值。</param>
        public RedDotNode(red_dot_type type, int value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        ///     获取红点类型。
        /// </summary>
        public red_dot_type Type { get; }

        /// <summary>
        ///     获取红点状态计数值。
        /// </summary>
        public int Value { get; }
    }
}