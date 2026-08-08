// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.User
{
    /// <summary>
    ///     玩家主角模块。
    /// </summary>
    [Module]
    public class UserModule : BaseModule
    {
        private long _clientTime;
        private long _serverTime;
        private long _serverTimeOffset;
        private long _userId;

        public void SetServerTime(long serverTime)
        {
            _serverTime = serverTime;
            _clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _serverTimeOffset = serverTime - _clientTime;
        }

        public void SetUserId(long userId)
        {
            _userId = userId;
        }

        public long GetUserId()
        {
            return _userId;
        }

        public long GetCurrentServerTime()
        {
            var currentLocalTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var currentServerTimestamp = currentLocalTimestamp + _serverTimeOffset;

            return currentServerTimestamp;
        }

        public void Init()
        {
        }
    }
}