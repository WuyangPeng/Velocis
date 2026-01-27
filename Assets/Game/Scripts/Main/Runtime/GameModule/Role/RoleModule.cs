using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Main.Runtime.GameModule.Role
{
    [Module]
    public class RoleModule : BaseModule
    {
        private int _changeCount;
        private bool _modifyName;
        private string _name = "";
        private int _perDayChangeCount;
        private string _surname = "";

        public void SetRole(role_response message)
        {
            _changeCount = message.ChangeCount;
            _modifyName = message.ModifyName;
            _name = message.Name;
            _perDayChangeCount = message.PerDayChangeCount;
            _surname = message.Surname;
        }

        public string GetName()
        {
            if (_modifyName)
            {
                return _name;
            }

            return GameEntry.Localization.GetString(_name);
        }

        public string GetSurname()
        {
            if (_modifyName)
            {
                return _surname;
            }

            return GameEntry.Localization.GetString(_surname);
        }

        public string GetFullName()
        {
            if (_modifyName)
            {
                return _surname + _name;
            }

            return GameEntry.Localization.GetString(_surname) + GameEntry.Localization.GetString(_name);
        }

        public void ChangeName(string surname, string name)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientRole_ChangeRoleName();
            request.Surname = surname;
            request.Name = name;

            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }
    }
}