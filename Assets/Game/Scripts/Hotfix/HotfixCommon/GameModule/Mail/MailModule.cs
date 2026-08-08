// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.GameModule.Base;
using Game.Scripts.Main.Runtime.Network;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Mail
{
    /// <summary>
    ///     邮件系统模块，管理所有邮件数据与向服务器发送邮件相关请求。
    /// </summary>
    [Module]
    public class MailModule : BaseModule
    {
        /// <summary>
        ///     获取所有邮件的内存数据字典。
        /// </summary>
        public Dictionary<long, MailData> Items { get; } = new();

        /// <summary>
        ///     清理所有邮件数据。
        /// </summary>
        public void ClearItems()
        {
            Items.Clear();
        }

        /// <summary>
        ///     删除指定唯一ID的邮件。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        public void DeleteItem(long mailId)
        {
            Items.Remove(mailId);
        }

        /// <summary>
        ///     获取指定ID的邮件数据。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        /// <returns>邮件数据实体，若未找到则返回 null。</returns>
        public MailData GetMail(long mailId)
        {
            Items.TryGetValue(mailId, out var mailData);
            return mailData;
        }

        /// <summary>
        ///     更新或添加一封邮件数据。
        /// </summary>
        /// <param name="mailData">新邮件数据。</param>
        public void UpdateMail(MailData mailData)
        {
            if (mailData != null)
            {
                Items[mailData.MailId] = mailData;
            }
        }

        /// <summary>
        ///     更新邮件的已读状态。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        /// <param name="read">是否已读。</param>
        public void UpdateMailReadStatus(long mailId, bool read)
        {
            if (Items.TryGetValue(mailId, out var mailData))
            {
                mailData.Read = read;
            }
        }

        /// <summary>
        ///     更新邮件的附件领取状态。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        /// <param name="collected">附件是否已被领取。</param>
        public void UpdateMailAttachmentCollected(long mailId, bool collected)
        {
            if (Items.TryGetValue(mailId, out var mailData))
            {
                mailData.AttachmentCollected = collected;
            }
        }

        /// <summary>
        ///     向服务器请求邮件列表。
        /// </summary>
        /// <param name="maxMailId">客户端当前已有的最大邮件ID。</param>
        /// <param name="languageType">多语言类型。</param>
        public void SendMailRequest(long maxMailId, int languageType)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_Mail();
            request.MaxMailId = maxMailId;
            request.LanguageType = languageType;
            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送读取单封邮件的请求。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        public void SendMailReadRequest(long mailId)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_MailRead();
            request.MailId = mailId;
            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送领取单封邮件附件的请求。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        public void SendMailCollectAttachmentRequest(long mailId)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_MailCollectAttachment();
            request.MailId = mailId;
            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送删除单封邮件的请求。
        /// </summary>
        /// <param name="mailId">邮件唯一ID。</param>
        public void SendMailDeleteRequest(long mailId)
        {
            var packet = ProtoHelper.GetProto();
            var request = packet.Mutable_ClientPlayer_ClientMail_MailDelete();
            request.MailId = mailId;
            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送一键领取所有邮件附件的请求。
        /// </summary>
        public void SendMailCollectAllAttachmentsRequest()
        {
            var packet = ProtoHelper.GetProto();
            packet.Mutable_ClientPlayer_ClientMail_MailCollectAllAttachments();
            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }

        /// <summary>
        ///     发送一键删除所有已读邮件的请求。
        /// </summary>
        public void SendMailDeleteAllReadRequest()
        {
            var packet = ProtoHelper.GetProto();
            packet.Mutable_ClientPlayer_ClientMail_MailDeleteAllRead();
            GameEntry.Network.Send(NetworkConstant.TcpChannel, packet);
        }
    }
}