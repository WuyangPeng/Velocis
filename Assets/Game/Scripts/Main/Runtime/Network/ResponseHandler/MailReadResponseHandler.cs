using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Mail;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class MailReadResponseHandler : CeleritasHandlerBase<mail_read_response>
    {
        private MailModule _mailModule;

        public override void Handle(object sender, header header, mail_read_response message)
        {
            if (message == null || message.MailId == 0)
            {
                Log.Warning("MailReadResponseHandler: Invalid mail read response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            _mailModule.UpdateMailReadStatus(message.MailId, true);
            Log.Info("MailReadResponseHandler: Updated mail read status for MailId={0}", message.MailId);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailReadResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule == null)
            {
                Log.Warning("MailModule is null in MailReadResponseHandler.EnsureModule.");
                return false;
            }

            return true;
        }
    }
}