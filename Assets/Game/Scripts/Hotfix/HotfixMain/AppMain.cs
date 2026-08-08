// 创建时间：2026-08-01
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Game.Scripts.HotFix.HotfixFramework.Runtime;

namespace Game.Scripts.Hotfix.HotfixMain
{
    /// <summary>
    ///     热更新程序集（DLL）的底层静态入口点。
    ///     主工程在加载完所有热更新程序集后，会首先通过反射调用此类的 Entrance 方法。
    ///     作为桥梁，它直接将调用转发给热更新框架层的真正入口 <see cref="HotfixEntry.Entrance" />。
    /// </summary>
    public static class AppMain
    {
        /// <summary>
        ///     热更新代码的启动方法。
        /// </summary>
        /// <param name="objects">参数列表。objects[0] 期望为 List{Assembly}，即已加载的热更新程序集列表。</param>
        public static void Entrance(object[] objects)
        {
            HotfixEntry.Entrance(objects);
        }
    }
}