using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Mail;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class MailCollectAllAttachmentsResponseHandler : CeleritasHandlerBase<mail_collect_all_attachments_response>
    {
        private MailModule _mailModule;

        public override void Handle(object sender, header header, mail_collect_all_attachments_response message)
        {
            if (message == null || message.MailIds.Count == 0)
            {
                Log.Info("MailCollectAllAttachmentsResponseHandler: No mail ids in collect all attachments response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            foreach (var mailId in message.MailIds)
            {
                _mailModule.UpdateMailAttachmentCollected(mailId, true);
            }

            Log.Info("MailCollectAllAttachmentsResponseHandler: Updated attachment collected status for {0} mails", message.MailIds.Count);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailCollectAllAttachmentsResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule != null)
            {
                return true;
            }

            Log.Warning("MailModule is null in MailCollectAllAttachmentsResponseHandler.EnsureModule.");
            return false;
        }
    }
}