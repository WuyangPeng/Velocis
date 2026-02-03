using Celeritas.Proto.Client;
using Celeritas.Proto.Common;
using System.Collections.Generic;

namespace Game.Scripts.Main.Runtime.GameModule.Mail
{
    public class MailData
    {
        public MailData()
        {
        }

        public MailData(mail_data protoData)
        {
            if (protoData != null)
            {
                MailId = protoData.MailId;
                Type = protoData.Type;
                Multilingual = protoData.Multilingual;
                Title = protoData.Title;
                Content = protoData.Content;
                SendTime = protoData.SendTime;
                ExpireTime = protoData.ExpireTime;
                Read = protoData.Read;
                AttachmentCollected = protoData.AttachmentCollected;
                
                // 复制附件数据
                Attachments = new List<inventory_data>();
                foreach (var attachment in protoData.Attachments)
                {
                    Attachments.Add(attachment.Clone());
                }
            }
        }

        public long MailId { get; set; }

        public int Type { get; set; }

        public bool Multilingual { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long SendTime { get; set; }

        public long ExpireTime { get; set; }

        public bool Read { get; set; }

        public bool AttachmentCollected { get; set; }

        public List<inventory_data> Attachments { get; set; } = new List<inventory_data>();

        public MailData Clone()
        {
            var clone = new MailData
            {
                MailId = MailId,
                Type = Type,
                Multilingual = Multilingual,
                Title = Title,
                Content = Content,
                SendTime = SendTime,
                ExpireTime = ExpireTime,
                Read = Read,
                AttachmentCollected = AttachmentCollected
            };

            foreach (var attachment in Attachments)
            {
                clone.Attachments.Add(attachment.Clone());
            }

            return clone;
        }

        public void Reset()
        {
            MailId = 0;
            Type = 0;
            Multilingual = false;
            Title = string.Empty;
            Content = string.Empty;
            SendTime = 0;
            ExpireTime = 0;
            Read = false;
            AttachmentCollected = false;
            Attachments.Clear();
        }

        public override string ToString()
        {
            return $"MailData(MailId={MailId}, Type={Type}, Title={Title}, Read={Read}, AttachmentCollected={AttachmentCollected})";
        }
    }
}