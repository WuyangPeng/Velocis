// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.RedDot
{
    /// <summary>
    ///     红点管理模块，用于维护和查询游戏内各红点节点的状态及数值。
    /// </summary>
    [Module]
    public class RedDotModule : BaseModule
    {
        private readonly Dictionary<red_dot_type, RedDotNode> _redDotNode = new();

        /// <summary>
        ///     添加或更新红点节点数据。
        /// </summary>
        /// <param name="node">红点节点。</param>
        public void AddRedDotNode(RedDotNode node)
        {
            _redDotNode[node.Type] = node;
        }

        /// <summary>
        ///     获取指定红点类型的数值（红点数量/状态）。
        /// </summary>
        /// <param name="type">红点类型。</param>
        /// <returns>红点当前的计数值，若无该节点则返回 0。</returns>
        public int GetRedDotNodeValue(red_dot_type type)
        {
            return _redDotNode.TryGetValue(type, out var node) ? node.Value : 0;
        }

        /// <summary>
        ///     清空所有保存在内存中的红点节点数据。
        /// </summary>
        public void ClearRedDotNode()
        {
            _redDotNode.Clear();
        }
    }
}