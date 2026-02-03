using System;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using Game.Scripts.Main.Runtime.GameModule.Mail;
using Game.Scripts.Main.Runtime.Network.PacketHandler;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Network.ResponseHandler
{
    public class MailResponseHandler : CeleritasHandlerBase<mail_response>
    {
        private MailModule _mailModule;

        public override void Handle(object sender, header header, mail_response message)
        {
            if (message == null || message.Mail.Count == 0)
            {
                Log.Info("MailResponseHandler: No mail data in response.");
                return;
            }

            if (!EnsureModule())
            {
                return;
            }

            StoreMailData(message.Mail);
        }

        private bool EnsureModule()
        {
            var moduleComponent = GameEntry.ModuleComponent;
            if (moduleComponent == null)
            {
                Log.Warning("ModuleComponent is null in MailResponseHandler.EnsureModule.");
                return false;
            }

            _mailModule ??= moduleComponent.GetModule<MailModule>();

            if (_mailModule == null)
            {
                Log.Warning("MailModule is null in MailResponseHandler.EnsureModule.");
                return false;
            }

            return true;
        }

        private void StoreMailData(RepeatedField<mail_data> mailList)
        {
            foreach (var mail in mailList)
            {
                try
                {
                    StoreMailData(mail);
                }
                catch (Exception ex)
                {
                    Log.Warning("Failed to process mail data: {0}", ex.Message);
                }
            }
        }

        private void StoreMailData(mail_data mail)
        {
            if (mail.MailId == 0)
            {
                Log.Warning("MailResponseHandler: ignoring mail data with no MailId: {0}", mail.ToString());
                return;
            }

            var mailData = new MailData(mail);
            _mailModule.UpdateMail(mailData);
            
            Log.Info("MailResponseHandler: Stored mail data for MailId={0}, Title={1}", mail.MailId, mail.Title);
        }
    }
}