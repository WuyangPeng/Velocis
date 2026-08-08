// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 玩家在该服务器上的角色基础数据。
    /// </summary>
    [Serializable]
    public class PlayerRole
    {
        /// <summary>玩家最后一次登录该服务器的时间戳。</summary>
        public long last_login_time;
        
        /// <summary>角色姓氏（如果包含）。</summary>
        public string role_surname_name = "";
        
        /// <summary>角色名字。</summary>
        public string role_name = "";
        
        /// <summary>是否修改过名字（或需要修改名字）。</summary>
        public bool modify_name;
    }
}
