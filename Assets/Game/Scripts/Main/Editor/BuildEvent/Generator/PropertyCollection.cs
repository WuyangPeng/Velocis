using System.Collections.Generic;
using Game.Scripts.Main.Runtime.RuntimeException;
using GameFramework;

namespace Game.Scripts.Main.Editor.BuildEvent.Generator
{
    /// <summary>
    ///     辅助类，用于收集和组织以数字结尾的同名前缀属性。
    /// </summary>
    public sealed class PropertyCollection
    {
        private readonly List<KeyValuePair<int, string>> _items;

        /// <summary>
        ///     初始化 PropertyCollection 的新实例。
        /// </summary>
        /// <param name="name">属性集合的名称（不含数字后缀）。</param>
        /// <param name="languageKeyword">属性的语言类型关键字（如 "int", "string"）。</param>
        public PropertyCollection(string name, string languageKeyword)
        {
            Name = name;
            LanguageKeyword = languageKeyword;
            _items = new List<KeyValuePair<int, string>>();
        }

        public string Name { get; }

        public string LanguageKeyword { get; }

        public int ItemCount => _items.Count;

        /// <summary>
        ///     按索引获取属性项。
        /// </summary>
        /// <param name="index">项的索引。</param>
        /// <returns>包含ID和属性名的键值对。</returns>
        public KeyValuePair<int, string> GetItem(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                throw new GameException(Utility.Text.Format("GetItem with invalid index '{0}'.", index));
            }

            return _items[index];
        }

        /// <summary>
        ///     向集合中添加一个属性项。
        /// </summary>
        /// <param name="id">属性的数字后缀ID。</param>
        /// <param name="propertyName">完整的属性名。</param>
        public void AddItem(int id, string propertyName)
        {
            _items.Add(new KeyValuePair<int, string>(id, propertyName));
        }
    }
}