using System;
using Celeritas.Config;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Develop;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class DevelopResponseHandler : CeleritasHandlerBase<develop_response>
    {
        private HeroDevelopModule _heroDevelopModule;

        public override void Handle(object sender, header header, develop_response message)
        {
            if (message.Develop.Count == 0)
            {
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            StoreDevelopData(message.Develop);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in DevelopResponseHandler.EnsureModule.");
                return false;
            }

            _heroDevelopModule ??= moduleComponent.GetModule<HeroDevelopModule>();

            if (_heroDevelopModule == null)
            {
                Log.Warning("HeroDevelopModule is null in DevelopResponseHandler.EnsureModule.");
                return false;
            }

            return true;
        }

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
                case develop_system_type.hero:
                {
                    _heroDevelopModule.Items[key] = data;
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