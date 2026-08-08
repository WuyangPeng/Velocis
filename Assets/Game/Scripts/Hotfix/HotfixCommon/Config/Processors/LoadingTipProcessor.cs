// 创建时间：2026-08-04
// 修改时间：2026-08-04
// 审核时间：

using System.Collections.Generic;
using Celeritas.Config;
using Celeritas.Config.game;
using UnityEngine;

namespace Game.Scripts.Hotfix.HotfixCommon.Config.Processors
{
    /// <summary>
    ///     登录加载提示语配置预处理器。
    /// </summary>
    public class LoadingTipProcessor : IConfigProcessor
    {
        private List<loading_tip_config> _configs;
        private List<int> _cumulativeWeights;

        /// <summary>
        ///     获取配置总权重。
        /// </summary>
        private int TotalWeight { get; set; }

        public void Process(tables tables)
        {
            _configs = tables.LoadingTipConfigContainer.DataList;
            _cumulativeWeights = new List<int>(_configs.Count);
            TotalWeight = 0;
            foreach (var config in _configs)
            {
                TotalWeight += config.Weight;
                _cumulativeWeights.Add(TotalWeight);
            }
        }

        /// <summary>
        ///     根据权重随机获取一个提示语配置。
        /// </summary>
        public loading_tip_config GetRandomTip()
        {
            if (TotalWeight <= 0)
            {
                return null;
            }

            var randomWeight = Random.Range(0, TotalWeight);
            return GetRandomTip(randomWeight);
        }

        /// <summary>
        ///     根据传入的随机数（0 到 TotalWeight - 1）获取对应的提示语配置。
        /// </summary>
        private loading_tip_config GetRandomTip(int randomWeight)
        {
            if (_configs == null || _configs.Count == 0 || _cumulativeWeights == null || _cumulativeWeights.Count == 0)
            {
                return null;
            }

            if (randomWeight < 0 || randomWeight >= TotalWeight)
            {
                throw new System.ArgumentOutOfRangeException(nameof(randomWeight), $"randomWeight must be between 0 and {TotalWeight - 1}. Actual: {randomWeight}");
            }

            // 使用二分查找快速定位累加权重区间
            var index = _cumulativeWeights.BinarySearch(randomWeight + 1);
            if (index < 0)
            {
                index = ~index;
            }

            return _configs[index];
        }
    }
}
