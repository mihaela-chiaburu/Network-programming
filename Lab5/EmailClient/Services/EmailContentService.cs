using EmailClient.Constants;
using EmailClient.Interfaces;
using EmailClient.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Services
{
    public class EmailContentService : IEmailContentService
    {
        public EmailDisplayInfo PrepareEmailForDisplay(MimeMessage message)
        {
            var displayInfo = new EmailDisplayInfo
            {
                From = message.From.ToString(),
                Subject = message.Subject,
                Date = message.Date.LocalDateTime.ToString("g"),
                HasAttachments = message.Attachments.GetEnumerator().MoveNext()
            };

            if (!string.IsNullOrEmpty(message.HtmlBody))
            {
                displayInfo.HtmlContent = message.HtmlBody;
            }
            else
            {
                displayInfo.HtmlContent = $"<pre>{message.TextBody}</pre>";
                displayInfo.TextContent = message.TextBody;
            }

            return displayInfo;
        }

        public ComposeEmailData PrepareReplyData(MimeMessage originalMessage, string userEmail)
        {
            return new ComposeEmailData
            {
                To = originalMessage.From.ToString(),
                Subject = AppConstants.Email.REPLY_PREFIX + originalMessage.Subject,
                ReplyTo = userEmail,
                Body = AppConstants.Email.ORIGINAL_MESSAGE_HEADER + originalMessage.TextBody
            };
        }
    }
}
