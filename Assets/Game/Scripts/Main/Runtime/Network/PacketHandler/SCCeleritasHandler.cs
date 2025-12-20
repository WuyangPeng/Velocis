using Celeritas.Proto;
using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Network.Packet;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.PacketHandler
{
    public class SCCeleritasHandler : PacketHandlerBase
    {
        public override int Id => 101;

        public override void Handle(object sender, GameFramework.Network.Packet packet)
        {
            var packetImpl = (SCCeleritas)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());

            if (packetImpl.Common.ToGateway.Code == 1)
            {
                switch (packetImpl.Celeritas.PayloadCase)
                {
                    case celeritas.PayloadOneofCase.CeleritasResponse:
                    {
                        var celeritasResponse = packetImpl.Celeritas.CeleritasResponse;
                        switch (celeritasResponse.PayloadCase)
                        {
                            case response.PayloadOneofCase.Client:
                            {
                                var client = celeritasResponse.Client;
                                switch (client.PayloadCase)
                                {
                                    case client_response.PayloadOneofCase.Player:
                                    {
                                        var player = client.Player;
                                        switch (player.PayloadCase)
                                        {
                                            case client_player_response.PayloadOneofCase.Login:
                                            {
                                                var login = player.Login;
                                                switch (login.PayloadCase)
                                                {
                                                    case client_login_response.PayloadOneofCase.Login:
                                                    {
                                                        var login_ = login.Login;
                                                        
                                                        var procedureMenu = (ProcedureMenu)GameEntry.Procedure.CurrentProcedure;
                                                        if (procedureMenu == null)
                                                        {
                                                            Log.Warning("ProcedureMenu is invalid when On Network Connected.");
                                                            return;
                                                        }
                                                        
                                                        procedureMenu.StartGame();

                                                        Log.Info("login_.CurrentTime ='{0}'.", login_.CurrentTime);
                                                    }
                                                        break;
                                                }
                                            }
                                                break;
                                        }
                                    }
                                        break;
                                }
                            }
                                break;
                        }

                        break;
                    }
                }
            }
            else
            {
                Log.Info("Receive packet Code ='{0}'.", packetImpl.Common.ToGateway.Code);
            }
        }
    }
}