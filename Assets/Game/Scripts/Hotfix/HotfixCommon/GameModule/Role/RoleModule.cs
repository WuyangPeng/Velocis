// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using Celeritas.Proto.Client;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Role
{
    /// <summary>
    ///     主角角色管理模块，负责角色起名、改名次数及姓名的获取。
    /// </summary>
    [Module]
    public class RoleModule : BaseModule
    {
        private int _changeCount;
        private bool _modifyName;
        private string _name = "";
        private int _perDayChangeCount;
        private string _surname = "";

        /// <summary>
        ///     根据服务器回复包数据更新角色姓名相关数据。
        /// </summary>
        /// <param name="message">服务器下发的角色数据。</param>
        public void SetRole(role_response message)
        {
            _changeCount = message.ChangeCount;
            _modifyName = message.ModifyName;
            _name = message.Name;
            _perDayChangeCount = message.PerDayChangeCount;
            _surname = message.Surname;
        }

        /// <summary>
        ///     获取角色名（不含姓氏）。
        /// </summary>
        /// <returns>名字，若是系统默认名则自动返回本地化后的字符串。</returns>
        public string GetName()
        {
            return _modifyName ? _name : GameEntry.Localization.GetString(_name);
        }

        /// <summary>
        ///     获取角色姓氏。
        /// </summary>
        /// <returns>姓氏，若是系统默认姓氏则自动返回本地化后的字符串。</returns>
        public string GetSurname()
        {
            return _modifyName ? _surname : GameEntry.Localization.GetString(_surname);
        }

        /// <summary>
        ///     获取角色全名（姓氏+名字）。
        /// </summary>
        /// <returns>全名字符串。</returns>
        public string GetFullName()
        {
            if (_modifyName)
            {
                return _surname + _name;
            }

            return GameEntry.Localization.GetString(_surname) + GameEntry.Localization.GetString(_name);
        }

        /// <summary>
        ///     向服务器发送请求修改角色姓名的协议消息。
        /// </summary>
        /// <param name="surname">新的姓氏。</param>
        /// <param name="name">新的名字。</param>
        public static void ChangeName(string surname, string name)
        {
            var packet = ProtoHelper.GetProto();

            var request = packet.Mutable_ClientPlayer_ClientRole_ChangeRoleName();
            request.Surname = surname;
            request.Name = name;

            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }
    }
}