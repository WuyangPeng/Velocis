// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Develop
{    
    /// <summary>
    /// 武将养成模块，管理所有武将的等级与养成数据。
    /// </summary>
    [Module]
    public class HeroDevelopModule: DevelopModule
    {
        /// <summary>
        /// 获取所有武将的养成数据字典。
        /// </summary>
        public Dictionary<long, DevelopData> Items { get; } = new();

        /// <summary>
        /// 清理所有武将的养成数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
        }

        /// <summary>
        /// 删除指定实例的武将养成数据。
        /// </summary>
        /// <param name="instanceId">武将数据实例ID。</param>
        public void DeleteItem(long instanceId)
        {
            Items.Remove(instanceId);
        }
    }
}