using System;

namespace Game.Scripts.Main.Runtime.Login
{
    [Serializable]
    public class ConnectionInfo
    {
        public string host;
        public int port;
        public ServerNetworkType server_network;
    }
}