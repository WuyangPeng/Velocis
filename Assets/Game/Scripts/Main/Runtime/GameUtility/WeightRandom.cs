using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Main.Runtime.RuntimeException;
using Random = UnityEngine.Random;

namespace Game.Scripts.Main.Runtime.GameUtility
{
    /// <summary>
    ///     通用权重工具类
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public sealed class WeightRandom<T>
    {
        private readonly List<Node> _nodes = new();
        private float _totalWeight;

        /// <summary>
        ///     当前元素个数
        /// </summary>
        public int Count => _nodes.Count;

        /// <summary>
        ///     总权重（只读）
        /// </summary>
        public double TotalWeight => _totalWeight;


        /// <summary>
        ///     添加元素（权重必须 > 0）
        /// </summary>
        public void Add(T value, float weight)
        {
            switch (weight)
            {
                case < 0:
                {
                    throw new GameException("添加元素的权重小于0。");
                }
                case > 0:
                {
                    _nodes.Add(new Node(value, weight));
                    _totalWeight += weight;
                    RecalcPrefix();
                    break;
                }
            }
        }

        /// <summary>
        ///     移除元素（返回是否成功）
        /// </summary>
        public bool Remove(T value)
        {
            var index = _nodes.FindIndex(n => EqualityComparer<T>.Default.Equals(n.Value, value));
            if (index < 0)
            {
                return false;
            }

            _totalWeight -= _nodes[index].Weight;
            _nodes.RemoveAt(index);
            RecalcPrefix();
            return true;
        }

        /// <summary>
        ///     清空所有元素
        /// </summary>
        public void Clear()
        {
            _nodes.Clear();
            _totalWeight = 0.0f;
        }

        /// <summary>
        ///     随机抽取一个元素（权重越大概率越高）
        /// </summary>
        public T Roll()
        {
            if (_nodes.Count == 0)
            {
                throw new GameException("集合为空。");
            }

            var value = Random.Range(0.0f, _totalWeight);
            var index = BinarySearch(value);
            return _nodes[index].Value;
        }

        /// <summary>
        ///     不放回抽取指定个数（不足则全返回）
        /// </summary>
        public List<T> RollMultiple(int count, bool allowDuplicate = false)
        {
            var result = new List<T>(count);

            if (allowDuplicate)
            {
                for (var i = 0; i < count; i++)
                {
                    result.Add(Roll());
                }

                return result;
            }

            if (_nodes.Count <= count)
            {
                result.AddRange(_nodes.Select(node => node.Value));
                return result;
            }

            // 不放回：权重拷贝后逐次删除
            var copy = new WeightRandom<T>();
            foreach (var node in _nodes)
            {
                copy.Add(node.Value, node.Weight);
            }

            var c = Math.Min(count, copy.Count);
            for (var i = 0; i < c; i++)
            {
                var picked = copy.Roll();
                result.Add(picked);
                copy.Remove(picked);
            }

            return result;
        }


        private void RecalcPrefix()
        {
            var sum = 0.0f;
            foreach (var node in _nodes)
            {
                sum += node.Weight;
                node.Prefix = sum;
            }
        }

        private int BinarySearch(double value)
        {
            var low = 0;
            var high = _nodes.Count - 1;
            while (low < high)
            {
                var mid = (low + high) >> 1;
                if (value < _nodes[mid].Prefix)
                {
                    high = mid;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return low;
        }

        private sealed class Node
        {
            public Node(T value, float weight)
            {
                Value = value;
                Weight = weight;
                Prefix = 0.0f;
            }

            public T Value { get; }
            public float Weight { get; }
            public float Prefix { get; internal set; }
        }
    }
}