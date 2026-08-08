// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using System.Linq;
using Celeritas.Config;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Develop
{
    /// <summary>
    /// 主角养成模块，管理角色的养成等级与相关事件派发。
    /// </summary>
    [Module]
    public class RoleDevelopModule : DevelopModule
    {
        private Dictionary<long, DevelopData> Items { get; } = new();

        /// <summary>
        /// 清理角色养成数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
        }

        /// <summary>
        /// 添加角色养成数据并派发等级改变事件。
        /// </summary>
        /// <param name="item">养成数据。</param>
        /// <param name="isLogin">是否在登录过程中加载数据。</param>
        public void AddItem(DevelopData item, bool isLogin)
        {
            Items.Add(item.InstanceId, item);

            if (!isLogin)
            {
                GameEntry.Event.Fire(this, ChangeDevelopLevelEventArgs.Create(develop_system_type.role));
            }
        }

        /// <summary>
        /// 删除指定实例的角色养成数据。
        /// </summary>
        /// <param name="instanceId">角色数据实例ID。</param>
        public void DeleteItem(long instanceId)
        {
            Items.Remove(instanceId);
        }

        /// <summary>
        /// 获取角色当前养成等级。
        /// </summary>
        /// <returns>角色的当前等级，如无数据则返回 0。</returns>
        public int GetLevel()
        {
            var firstItem = Items.Values.FirstOrDefault();
            return firstItem?.Level ?? 0;
        }
    }
}