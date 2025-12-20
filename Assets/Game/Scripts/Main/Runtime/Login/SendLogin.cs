using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Network;
using UnityEngine.Device;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Login
{
    public static class SendLogin
    {
        public static void SendMessage()
        {
            var packet = ProtoHelper.GetProto();

            var login = packet.SetPlayerLogin();

            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            login.Token = accountModule.GetToken();
            login.GameServerId = accountModule.GetCurrentGameServerId();
            login.DeviceId = SystemInfo.deviceUniqueIdentifier;
            login.AppVersion = GameEntry.Account.appVersion;

            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }
    }
}