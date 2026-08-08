// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.Develop;
using Game.Scripts.Hotfix.HotfixCommon.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Network.ResponseHandler
{
    /// <summary>
    ///     养成系统数据响应处理器。
    /// </summary>
    public class DevelopResponseHandler : CeleritasHandlerBase<develop_response>
    {
        private HeroDevelopModule _heroDevelopModule;
        private bool _isLogin;
        private RoleDevelopModule _roleDevelopModule;
        private VipDevelopModule _vipDevelopModule;


        protected override void Handle(object sender, header header, develop_response message)
        {
            _isLogin = message.IsLogin;

            if (message.Develop.Count == 0)
            {
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            if (_isLogin)
            {
                ClearAllDevelopModules();
            }

            StoreDevelopData(message.Develop);
        }

        /// <summary>
        ///     登录刷新时，清理所有养成模块中已存在的数据。
        /// </summary>
        private void ClearAllDevelopModules()
        {
            _roleDevelopModule?.ClearItems();
            _vipDevelopModule?.ClearItems();
            _heroDevelopModule?.ClearItems();

            Log.Info("DevelopResponse: cleared all module item collections due to login refresh (IsLogin=true).");
        }

        /// <summary>
        ///     确保相关的养成系统模块实例已加载。
        /// </summary>
        /// <returns>若模块加载成功返回 true，否则返回 false。</returns>
        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in DevelopResponseHandler.EnsureModule.");
                return false;
            }

            _roleDevelopModule ??= moduleComponent.GetModule<RoleDevelopModule>();

            if (_roleDevelopModule == null)
            {
                Log.Warning("RoleDevelopModule is null in DevelopResponseHandler.EnsureModule.");
                return false;
            }

            _vipDevelopModule ??= moduleComponent.GetModule<VipDevelopModule>();

            if (_vipDevelopModule == null)
            {
                Log.Warning("VipDevelopModule is null in DevelopResponseHandler.EnsureModule.");
                return false;
            }

            _heroDevelopModule ??= moduleComponent.GetModule<HeroDevelopModule>();

            if (_heroDevelopModule != null)
            {
                return true;
            }

            Log.Warning("HeroDevelopModule is null in DevelopResponseHandler.EnsureModule.");
            return false;
        }

        /// <summary>
        ///     批量存储养成数据。
        /// </summary>
        /// <param name="developList">养成数据列表。</param>
        private void StoreDevelopData(RepeatedField<develop_data> developList)
        {
            foreach (var develop in developList)
            {
                try
                {
                    StoreDevelopData(develop);
                }
                catch (Exception ex)
                {
                    Log.Warning("Failed to process develop data: {0}", ex.Message);
                }
            }
        }

        /// <summary>
        ///     存储单个养成数据，并分发给对应的系统模块（角色/Vip/英雄）。
        /// </summary>
        /// <param name="develop">单个养成数据结构。</param>
        private void StoreDevelopData(develop_data develop)
        {
            if (develop.InstanceId == 0)
            {
                Log.Warning("DevelopResponseHandler: ignoring develop data with no InstanceId: {0}", develop.ToString());
                return;
            }

            var key = develop.InstanceId;
            var data = new DevelopData(develop);

            switch ((develop_system_type)develop.SystemId)
            {
                case develop_system_type.role:
                {
                    _roleDevelopModule.AddItem(data, _isLogin);
                    break;
                }
                case develop_system_type.vip:
                {
                    _vipDevelopModule.AddItem(data, _isLogin);
                    break;
                }
                case develop_system_type.hero:
                {
                    _heroDevelopModule.Items[key] = data;
                    break;
                }
                case develop_system_type.building:
                {
                    break;
                }
                case develop_system_type.tech:
                {
                    break;
                }
                case develop_system_type.none:
                default:
                {
                    Log.Warning("invalid develop system type: {0}", develop.ToString());
                    return;
                }
            }
        }
    }
}