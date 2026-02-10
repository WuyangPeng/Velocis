using System;

namespace Game.Scripts.Main.Runtime.Login
{
    [Serializable]
    public class PlayerRole
    {
        public long last_login_time;
        public string role_surname_name;
        public string role_name;
        public bool modify_name;
    }
}