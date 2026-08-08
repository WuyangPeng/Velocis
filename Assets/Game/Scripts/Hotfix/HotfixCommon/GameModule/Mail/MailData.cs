// 创建时间：2026-07-26
// 修改时间：2026-08-01
// 审核时间：2026-08-01

using System.Collections.Generic;
using Celeritas.Proto.Client;
using Celeritas.Proto.Common;

namespace Game.Scripts.Hotfix.HotfixCommon.GameModule.Mail
{
    /// <summary>
    ///     邮件数据实体类。
    /// </summary>
    public class MailData
    {
        /// <summary>
        ///     初始化 <see cref="MailData" /> 类的新实例。
        /// </summary>
        public MailData()
        {
        }

        /// <summary>
        ///     初始化 <see cref="MailData" /> 类的新实例。
        /// </summary>
        /// <param name="protoData">服务器下发的网络协议数据。</param>
        public MailData(mail_data protoData)
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

        /// <summary>
        ///     获取或设置邮件唯一编号。
        /// </summary>
        public long MailId { get; set; }

        /// <summary>
        ///     获取或设置邮件类型。
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///     获取或设置是否为多语言邮件。
        /// </summary>
        public bool Multilingual { get; set; }

        /// <summary>
        ///     获取或设置邮件标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     获取或设置邮件内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     获取或设置发送时间戳。
        /// </summary>
        public long SendTime { get; set; }

        /// <summary>
        ///     获取或设置过期时间戳。
        /// </summary>
        public long ExpireTime { get; set; }

        /// <summary>
        ///     获取或设置邮件是否已读。
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        ///     获取或设置附件是否已领。
        /// </summary>
        public bool AttachmentCollected { get; set; }

        /// <summary>
        ///     获取或设置附件数据列表。
        /// </summary>
        public List<inventory_data> Attachments { get; set; } = new();

        /// <summary>
        ///     克隆当前邮件数据。
        /// </summary>
        /// <returns>新的 MailData 实例。</returns>
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

        /// <summary>
        ///     重置所有邮件数据为默认值。
        /// </summary>
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

        /// <summary>
        ///     返回当前对象的字符串表示形式。
        /// </summary>
        /// <returns>格式化后的字符串。</returns>
        public override string ToString()
        {
            return $"MailData(MailId={MailId}, Type={Type}, Title={Title}, Read={Read}, AttachmentCollected={AttachmentCollected})";
        }
    }
}