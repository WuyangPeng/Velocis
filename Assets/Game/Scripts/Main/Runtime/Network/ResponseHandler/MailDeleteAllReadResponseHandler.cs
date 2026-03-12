using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Mail;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class MailDeleteAllReadResponseHandler : CeleritasHandlerBase<mail_delete_all_read_response>
    {
        private MailModule _mailModule;

        public override void Handle(object sender, header header, mail_delete_all_read_response message)
        {
            if (message == null || message.MailIds.Count == 0)
            {
                Log.Info("MailDeleteAllReadResponseHandler: No mail ids in delete all read response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            foreach (var mailId in message.MailIds)
            {
                _mailModule.DeleteItem(mailId);
            }

            Log.Info("MailDeleteAllReadResponseHandler: Deleted {0} read mails", message.MailIds.Count);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailDeleteAllReadResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule != null)
            {
                return true;
            }

            Log.Warning("MailModule is null in MailDeleteAllReadResponseHandler.EnsureModule.");
            return false;
        }
    }
}