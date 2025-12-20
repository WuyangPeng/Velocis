using System;
using Celeritas.Proto;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using ProtoBuf;

namespace Game.Scripts.Main.Runtime.Network.Packet
{
    [Serializable]
    [ProtoContract(Name = @"CSCeleritas")]
    public class CSCeleritas : CSPacketBase
    {
        public CSCeleritas()
        {
            Common = new header();
            Celeritas = new celeritas
            {
                CeleritasRequest = new request
                {
                    Client = new client_request()
                }
            };
        }

        public override int Id => 100;

        public header Common { get; set; }
        public celeritas Celeritas { get; set; }

        public client_player_request SetPlayer()
        {
            Celeritas.CeleritasRequest.Client.Player = new client_player_request();

            return Celeritas.CeleritasRequest.Client.Player;
        }

        public client_login_request SetPlayerClientLogin()
        {
            SetPlayer();
            Celeritas.CeleritasRequest.Client.Player.Login = new client_login_request();

            return Celeritas.CeleritasRequest.Client.Player.Login;
        }

        public login_request SetPlayerLogin()
        {
            SetPlayerClientLogin();
            Celeritas.CeleritasRequest.Client.Player.Login.Login = new login_request();

            return Celeritas.CeleritasRequest.Client.Player.Login.Login;
        }

        public client_heartbeat_request SetPlayerClientHeartbeat()
        {
            SetPlayer();
            Celeritas.CeleritasRequest.Client.Player.Heartbeat = new client_heartbeat_request();

            return Celeritas.CeleritasRequest.Client.Player.Heartbeat;
        }

        public override void Clear()
        {
            Common = new header();
            Celeritas = new celeritas
            {
                CeleritasRequest = new request
                {
                    Client = new client_request()
                }
            };
        }
    }
}
