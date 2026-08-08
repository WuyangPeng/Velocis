// 创建时间：2026-07-26
// 修改时间：2026-08-03
// 审核时间：2026-08-03

using Celeritas.Config;
using Game.Scripts.Main.Runtime.Config;

namespace Game.Scripts.Hotfix.HotfixCommon.Config
{
    /// <summary>
    /// 游戏配置组件扩展类，便于在热更新层方便地通过配置组件获取热更新配置表实例。
    /// </summary>
    public static class GameConfigComponentExtension
    {
        /// <summary>
        /// 获取热更新程序集中的游戏配置实例。
        /// </summary>
        /// <param name="component">主工程的配置组件实例。</param>
        /// <returns>热更新配置表实例。</returns>
        private static GameConfig GetGameConfig(this GameConfigComponent component)
        {
            return component.ConfigInstance as GameConfig;
        }

        /// <summary>
        /// 直接获取 Luban 配置表实例。
        /// </summary>
        /// <param name="component">主工程的配置组件实例。</param>
        /// <returns>Luban 配置表实例。</returns>
        public static tables GetTables(this GameConfigComponent component)
        {
            return component.GetGameConfig()?.GetTables();
        }

        /// <summary>
        /// 获取热更新配置预处理器实例。
        /// </summary>
        /// <typeparam name="T">预处理器类型。</typeparam>
        /// <param name="component">配置组件。</param>
        /// <returns>预处理器实例。</returns>
        public static T GetProcessor<T>(this GameConfigComponent component) where T : class, IConfigProcessor
        {
            return component.GetGameConfig()?.GetProcessor<T>();
        }
    }
}
