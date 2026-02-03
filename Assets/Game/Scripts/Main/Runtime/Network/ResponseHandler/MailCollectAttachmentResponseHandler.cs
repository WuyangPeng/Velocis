using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Mail;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class MailCollectAttachmentResponseHandler : CeleritasHandlerBase<mail_collect_attachment_response>
    {
        private MailModule _mailModule;

        public override void Handle(object sender, header header, mail_collect_attachment_response message)
        {
            if (message == null || message.MailId == 0)
            {
                Log.Warning("MailCollectAttachmentResponseHandler: Invalid mail collect attachment response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            _mailModule.UpdateMailAttachmentCollected(message.MailId, true);
            Log.Info("MailCollectAttachmentResponseHandler: Updated mail attachment collected status for MailId={0}", message.MailId);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailCollectAttachmentResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule == null)
            {
                Log.Warning("MailModule is null in MailCollectAttachmentResponseHandler.EnsureModule.");
                return false;
            }

            return true;
        }
    }
}