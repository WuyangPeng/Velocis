// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Proto.Client;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Develop
{
    /// <summary>
    /// 养成系统数据实体类。
    /// </summary>
    public class DevelopData
    {
        /// <summary>
        /// 初始化 <see cref="DevelopData"/> 类的新实例。
        /// </summary>
        public DevelopData()
        {
        }

        /// <summary>
        /// 初始化 <see cref="DevelopData"/> 类的新实例。
        /// </summary>
        /// <param name="systemId">系统类型ID。</param>
        /// <param name="instanceId">数据实例唯一ID。</param>
        /// <param name="level">当前养成等级。</param>
        /// <param name="exp">当前累计经验值。</param>
        public DevelopData(int systemId, long instanceId, int level, long exp)
        {
            SystemId = systemId;
            InstanceId = instanceId;
            Level = level;
            Exp = exp;
        }

        /// <summary>
        /// 初始化 <see cref="DevelopData"/> 类的新实例。
        /// </summary>
        /// <param name="protoData">服务器下发的网络协议数据。</param>
        public DevelopData(develop_data protoData)
        {
            SystemId = protoData.SystemId;
            InstanceId = protoData.InstanceId;
            Level = protoData.Level;
            Exp = protoData.Exp;
        }

        /// <summary>
        /// 获取或设置系统类型ID。
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 获取或设置数据实例ID。
        /// </summary>
        public long InstanceId { get; set; }

        /// <summary>
        /// 获取或设置当前养成等级。
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 获取或设置当前累计经验值。
        /// </summary>
        public long Exp { get; set; }

        /// <summary>
        /// 克隆当前数据。
        /// </summary>
        /// <returns>新的 DevelopData 实例。</returns>
        public DevelopData Clone()
        {
            return new DevelopData(SystemId, InstanceId, Level, Exp);
        }

        /// <summary>
        /// 重置所有数据为默认值。
        /// </summary>
        public void Reset()
        {
            SystemId = 0;
            InstanceId = 0;
            Level = 0;
            Exp = 0;
        }

        /// <summary>
        /// 返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"DevelopData(SystemId={SystemId}, InstanceId={InstanceId}, Level={Level}, Exp={Exp})";
        }
    }
}