// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Config;
using GameFramework.Event;

namespace Game.Scripts.Hotfix.HotfixCommon.Event
{
    /// <summary>
    /// 养成等级改变事件参数。
    /// </summary>
    public class ChangeDevelopLevelEventArgs : GameEventArgs
    {
        /// <summary>
        /// 养成等级改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(ChangeDevelopLevelEventArgs).GetHashCode();

        private ChangeDevelopLevelEventArgs(develop_system_type systemType)
        {
            SystemType = systemType;
        }

        /// <summary>
        /// 获取系统类型。
        /// </summary>
        public develop_system_type SystemType { get; private set; }

        /// <summary>
        /// 获取事件编号。
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 创建养成等级改变事件。
        /// </summary>
        /// <param name="systemType">系统类型。</param>
        /// <returns>创建的事件参数。</returns>
        public static ChangeDevelopLevelEventArgs Create(develop_system_type systemType)
        {
            return new ChangeDevelopLevelEventArgs(systemType);
        }

        /// <summary>
        /// 清理养成等级改变事件。
        /// </summary>
        public override void Clear()
        {
            SystemType = develop_system_type.none;
        }
    }
}