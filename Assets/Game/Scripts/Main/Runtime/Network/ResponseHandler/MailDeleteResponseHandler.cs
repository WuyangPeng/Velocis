using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Mail;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class MailDeleteResponseHandler : CeleritasHandlerBase<mail_delete_response>
    {
        private MailModule _mailModule;

        public override void Handle(object sender, header header, mail_delete_response message)
        {
            if (message == null || message.MailId == 0)
            {
                Log.Warning("MailDeleteResponseHandler: Invalid mail delete response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            _mailModule.DeleteItem(message.MailId);
            Log.Info("MailDeleteResponseHandler: Deleted mail for MailId={0}", message.MailId);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailDeleteResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule != null)
            {
                return true;
            }

            Log.Warning("MailModule is null in MailDeleteResponseHandler.EnsureModule.");
            return false;
        }
    }
}