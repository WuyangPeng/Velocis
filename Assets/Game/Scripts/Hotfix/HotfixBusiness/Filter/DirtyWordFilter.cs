// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using System.Linq;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Hotfix.HotfixBusiness.Filter
{
    /// <summary>
    ///     Trie 敏感词过滤器。支持检测与星号替换。
    /// </summary>
    public sealed class DirtyWordFilter
    {
        /// <summary>
        ///     单例实例。
        /// </summary>
        private static DirtyWordFilter _instance;

        /// <summary>
        ///     前缀树（Trie）的根节点。
        /// </summary>
        private readonly TrieNode _root = new();

        /// <summary>
        ///     获取 DirtyWordFilter 的单例实例。
        /// </summary>
        public static DirtyWordFilter Instance => _instance ??= new DirtyWordFilter();

        /// <summary>
        ///     获取敏感词过滤器是否已成功加载数据。
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        ///     从文本内容中加载并构建敏感词前缀树。
        ///     每一行代表一个敏感词，以 '#' 开头的行为注释行。
        /// </summary>
        /// <param name="text">包含敏感词列表的文本数据。</param>
        public void LoadFromText(string text)
        {
            _root.children.Clear();
            _root.isEnd = false;
            IsLoaded = false;

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (line.Length == 0 || line[0] == '#')
                {
                    continue;
                }

                Insert(line);
            }

            IsLoaded = true;
        }

        /// <summary>
        ///     检测输入字符串中是否包含敏感词。
        /// </summary>
        /// <param name="input">待检测的字符串。</param>
        /// <returns>若包含至少一个敏感词，则返回 true；否则返回 false。</returns>
        public bool ContainsDirtyWord(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (IsLoaded)
            {
                return input.Where((t, i) => CheckMatch(input, i)).Any();
            }

            Log.Warning("DirtyWordFilter is not loaded yet! Blocked input by default.");
            return true;

        }

        /// <summary>
        ///     替换输入字符串中的所有敏感词为指定的屏蔽字符。
        /// </summary>
        /// <param name="input">待过滤的字符串。</param>
        /// <param name="mask">用于替换敏感词字符的掩码字符，默认为 '*'。</param>
        /// <returns>替换完成后的字符串。</returns>
        public string ReplaceDirtyWords(string input, char mask = '*')
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (!IsLoaded)
            {
                Log.Warning("DirtyWordFilter is not loaded yet! Replaced input with mask by default.");
                return new string(mask, input.Length);
            }

            var chars = input.ToCharArray();
            for (var i = 0; i < chars.Length; ++i)
            {
                var end = GetMatchEndIndex(input, i);
                if (end < i)
                {
                    continue;
                }

                for (var k = i; k <= end; k++)
                {
                    chars[k] = mask;
                }

                i = end; // 跳过已屏蔽的字符
            }

            return new string(chars);
        }

        /// <summary>
        ///     从指定索引开始，检测是否存在匹配的敏感词。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <param name="startIndex">起始检测索引。</param>
        /// <returns>若存在匹配的敏感词，则返回 true；否则返回 false。</returns>
        private bool CheckMatch(string input, int startIndex)
        {
            var node = _root;
            for (var i = startIndex; i < input.Length; ++i)
            {
                if (!node.children.TryGetValue(input[i], out node))
                {
                    break;
                }

                if (node.isEnd)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     从指定索引开始，查找匹配的最长敏感词的结束索引。
        /// </summary>
        /// <param name="input">输入字符串。</param>
        /// <param name="startIndex">起始检测索引。</param>
        /// <returns>若找到匹配敏感词，返回最长匹配词的结束索引；否则返回 -1。</returns>
        private int GetMatchEndIndex(string input, int startIndex)
        {
            var node = _root;
            var end = -1;
            for (var i = startIndex; i < input.Length; ++i)
            {
                if (!node.children.TryGetValue(input[i], out node))
                {
                    break;
                }

                if (node.isEnd)
                {
                    end = i;
                }
            }

            return end;
        }

        /// <summary>
        ///     向前缀树中插入一个敏感词。
        /// </summary>
        /// <param name="word">需要插入的敏感词。</param>
        private void Insert(string word)
        {
            var node = _root;
            foreach (var c in word)
            {
                if (!node.children.TryGetValue(c, out var next))
                {
                    next = new TrieNode();
                    node.children[c] = next;
                }

                node = next;
            }

            node.isEnd = true;
        }
    }
}