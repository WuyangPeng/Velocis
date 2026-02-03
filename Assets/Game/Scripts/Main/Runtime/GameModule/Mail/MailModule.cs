using System.Collections.Generic;
using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Mail
{
    [Module]
    public class MailModule : BaseModule
    {
        public Dictionary<long, MailData> Items { get; } = new();

        public void ClearItems()
        {
            Items.Clear();
        }

        public void DeleteItem(long mailId)
        {
            Items.Remove(mailId);
        }

        public MailData GetMail(long mailId)
        {
            Items.TryGetValue(mailId, out var mailData);
            return mailData;
        }

        public void UpdateMail(MailData mailData)
        {
            if (mailData != null)
            {
                Items[mailData.MailId] = mailData;
            }
        }

        public void UpdateMailReadStatus(long mailId, bool read)
        {
            if (Items.TryGetValue(mailId, out var mailData))
            {
                mailData.Read = read;
            }
        }

        public void UpdateMailAttachmentCollected(long mailId, bool collected)
        {
            if (Items.TryGetValue(mailId, out var mailData))
            {
                mailData.AttachmentCollected = collected;
            }
        }
    }
}
