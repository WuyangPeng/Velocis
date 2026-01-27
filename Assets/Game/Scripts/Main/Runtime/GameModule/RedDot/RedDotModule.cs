using System.Collections.Generic;
using Celeritas.Config;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.RedDot
{
    [Module]
    public class RedDotModule : BaseModule
    {
        private readonly Dictionary<red_dot_type, RedDotNode> _redDotNode = new();

        public void AddRedDotNode(RedDotNode node)
        {
            _redDotNode[node.Type] = node;
        }

        public void ClearRedDotNode()
        {
            _redDotNode.Clear();
        }
    }
}