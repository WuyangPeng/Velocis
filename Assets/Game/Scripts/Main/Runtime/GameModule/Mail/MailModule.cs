using System.Collections.Generic;
using Celeritas.Proto.Client;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.Network;
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
        
        public void SendMailRequest(long maxMailId, int languageType)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_Mail();
            request.MaxMailId = maxMailId;
            request.LanguageType = languageType;
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }

        public void SendMailReadRequest(long mailId)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_MailRead();
            request.MailId = mailId;
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }

        public void SendMailCollectAttachmentRequest(long mailId)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_MailCollectAttachment();
            request.MailId = mailId;
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }

        public void SendMailDeleteRequest(long mailId)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_MailDelete();
            request.MailId = mailId;
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }

        public void SendMailCollectAllAttachmentsRequest()
        {
            var packet = ProtoHelper.GetProto();
            packet.Mutable_ClientPlayer_ClientMail_MailCollectAllAttachments();
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }

        public void SendMailDeleteAllReadRequest()
        {
            var packet = ProtoHelper.GetProto();
            packet.Mutable_ClientPlayer_ClientMail_MailDeleteAllRead();
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel");
            channel.Send(packet);
        }
    }
}
