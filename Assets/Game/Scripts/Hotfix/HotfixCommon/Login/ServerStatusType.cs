// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 游戏服务器运行状态类型。
    /// </summary>
    public enum ServerStatusType
    {
        /// <summary>流畅状态。</summary>
        Normal = 0,
        
        /// <summary>繁忙状态。</summary>
        Busy = 1,
        
        /// <summary>拥挤状态。</summary>
        Crowded = 2,
        
        /// <summary>爆满状态。</summary>
        Full = 3,
        
        /// <summary>维护中状态。</summary>
        Maintenance = 4
    }
}
