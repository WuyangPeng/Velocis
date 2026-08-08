// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Login
{
    /// <summary>
    /// 登录服务器的详细信息。
    /// </summary>
    [Serializable]
    public class LoginServerInfo
    {
        /// <summary>服务器的连接配置信息。</summary>
        public ConnectionInfo connection_info;
        
        /// <summary>游戏服务器的唯一标识 ID。</summary>
        public string game_server_id = "";
        
        /// <summary>玩家在当前服务器上的角色信息（如果有）。</summary>
        public PlayerRole player_role;
        
        /// <summary>服务器的显示名称。</summary>
        public string server_name = "";
        
        /// <summary>服务器的当前运行状态。</summary>
        public ServerStatusType server_status;
        
        /// <summary>服务器所属的分区 ID。</summary>
        public int zone = 0;

        /// <summary>
        /// 用 game_server_id 作为多语言 key 后缀查找服务器显示名。
        /// key 格式：ServerList.Server.{game_server_id}，如 ServerList.Server.test-1
        /// </summary>
        public string GetDisplayServerName()
        {
            return GameEntry.Localization.GetString($"ServerList.Server.{game_server_id}");
        }

        public string GetPlayerName()
        {
            if (player_role == null)
            {
                return "";
            }

            if (player_role.modify_name)
            {
                return player_role.role_surname_name + player_role.role_name;
            }

            if (player_role.role_surname_name.Length == 0)
            {
                return player_role.role_name.Length == 0 ? "" : GameEntry.Localization.GetString(player_role.role_name);
            }

            return GameEntry.Localization.GetString(player_role.role_surname_name) +
                   GameEntry.Localization.GetString(player_role.role_name);
        }
    }
}
