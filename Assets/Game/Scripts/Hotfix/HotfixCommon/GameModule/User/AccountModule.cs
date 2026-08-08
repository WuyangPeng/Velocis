// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Hotfix.HotfixCommon.Login;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.User
{
    /// <summary>
    ///     账户管理模块，维护登录状态、Token、可用的服务器列表及当前选中的服务器信息。
    /// </summary>
    [Module]
    public class AccountModule : BaseModule
    {
        private int _currentIndex = -1;
        private LoginServerInfo _currentLoginServerInfo;
        private List<LoginServerInfo> _loginServerInfo = new();
        private Token _token = new();
        private List<string> _zones = new();

        /// <summary>
        ///     设置当前登录的 Token 及其过期时间。
        /// </summary>
        /// <param name="token">登录 Token 字符串。</param>
        /// <param name="expireMilliseconds">过期时间戳（毫秒）。</param>
        public void SetToken(string token, long expireMilliseconds)
        {
            _token.SetToken(token, expireMilliseconds);
        }

        /// <summary>
        ///     设置可用的登录服务器列表，并将当前选中服务器重置为空。
        /// </summary>
        /// <param name="loginServerInfo">服务器列表数据。</param>
        public void SetLoginServerInfo(List<LoginServerInfo> loginServerInfo)
        {
            _loginServerInfo = loginServerInfo;
            _currentLoginServerInfo = null;
        }

        /// <summary>
        ///     设置服务器分区/大区列表数据。
        /// </summary>
        /// <param name="zones">大区列表。</param>
        public void SetZones(List<string> zones)
        {
            _zones = zones ?? new List<string>();
        }

        /// <summary>
        ///     获取所有大区列表。
        /// </summary>
        /// <returns>大区名称列表。</returns>
        public List<string> GetZones()
        {
            return _zones;
        }

        /// <summary>
        ///     获取当前有效的登录 Token 字符串。
        /// </summary>
        /// <returns>Token 字符串。</returns>
        public string GetToken()
        {
            return _token.GetToken();
        }

        /// <summary>
        ///     获取当前选中的游戏服务器唯一ID。
        /// </summary>
        /// <returns>服务器 ID。</returns>
        public string GetCurrentGameServerId()
        {
            return _currentLoginServerInfo.game_server_id;
        }

        /// <summary>
        ///     获取当前选中的游戏服务器名称。
        /// </summary>
        /// <returns>服务器名称。</returns>
        public string GetCurrentGameServerName()
        {
            return _currentLoginServerInfo.server_name;
        }

        /// <summary>
        ///     获取可用的登录服务器列表数据。
        /// </summary>
        /// <returns>服务器列表。</returns>
        public List<LoginServerInfo> GetLoginServerInfo()
        {
            return _loginServerInfo;
        }

        /// <summary>
        ///     清空账户模块所有相关状态数据，返回至未登录状态。
        /// </summary>
        public void Clear()
        {
            _currentLoginServerInfo = null;
            _loginServerInfo.Clear();
            _token = new Token();
            _zones.Clear();
            _currentIndex = -1;
        }

        /// <summary>
        ///     清理当前选中的服务器数据。
        /// </summary>
        public void ClearCurrentLogin()
        {
            _currentLoginServerInfo = null;
            _currentIndex = -1;
        }

        /// <summary>
        ///     获取当前选中服务器在列表中的索引。
        /// </summary>
        /// <returns>索引值。</returns>
        public int GetCurrentIndex()
        {
            return _currentIndex;
        }

        /// <summary>
        ///     设置当前选中的服务器索引，并返回该服务器的信息。
        /// </summary>
        /// <param name="index">服务器列表中的索引。</param>
        /// <returns>选中的服务器信息实体。</returns>
        public LoginServerInfo SetCurrentLoginServerInfo(int index)
        {
            _currentIndex = index;

            _currentLoginServerInfo = _loginServerInfo[index];

            return _currentLoginServerInfo;
        }
    }
}