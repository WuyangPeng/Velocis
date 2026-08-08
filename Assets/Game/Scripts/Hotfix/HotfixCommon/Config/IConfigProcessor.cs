// 创建时间：2026-08-03
// 修改时间：2026-08-03
// 审核时间：2026-08-03

namespace Game.Scripts.Hotfix.HotfixCommon.Config
{
    /// <summary>
    /// 配置表预处理器接口。
    /// </summary>
    public interface IConfigProcessor
    {
        /// <summary>
        /// 执行预处理逻辑。
        /// </summary>
        /// <param name="tables">已加载的 Luban 配置表实例。</param>
        void Process(Celeritas.Config.tables tables);
    }
}
