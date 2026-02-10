using System;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Login
{
    [Serializable]
    public class LoginServerInfo
    {
        public ConnectionInfo connection_info;
        public string game_server_id = "";
        public PlayerRole player_role;
        public string server_name = "";
        public ServerStatusType server_status;

        public string getPlayerName()
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