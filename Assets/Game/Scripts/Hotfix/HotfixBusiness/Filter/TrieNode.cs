// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System.Collections.Generic;

namespace Game.Scripts.Hotfix.HotfixBusiness.Filter
{
    /// <summary>
    /// 前缀树节点。
    /// </summary>
    internal sealed class TrieNode
    {
        /// <summary>
        /// 子节点字典，Key 为字符，Value 为对应的子节点。
        /// </summary>
        public readonly Dictionary<char, TrieNode> children = new();

        /// <summary>
        /// 是否是某个敏感词的末尾字符。
        /// </summary>
        public bool isEnd;
    }
}
