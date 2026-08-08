// 创建时间：2026-08-07
// 修改时间：2026-08-07

using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.Tools.Button
{
    /// <summary>
    /// 单个悬停粒子的运行时状态数据结构
    /// </summary>
    internal struct ParticleState
    {
        /// <summary> 粒子的 UI Image 组件 </summary>
        public Image image;
        
        /// <summary> 粒子的 RectTransform 组件 </summary>
        public RectTransform rect;
        
        /// <summary> 动画起始位置（相对 Local 坐标） </summary>
        public Vector2 startPosition;
        
        /// <summary> 动画终点位置（相对 Local 坐标） </summary>
        public Vector2 endPosition;
        
        /// <summary> 单次动画总持续时间（秒） </summary>
        public float duration;
        
        /// <summary> 当前动画已播放时长（秒） </summary>
        public float elapsedTime;
        
        /// <summary> 粒子尺寸大小 </summary>
        public float size;
        
        /// <summary> 粒子自转速度（度/秒） </summary>
        public float spinSpeed;
    }
}