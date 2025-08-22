using EmailClient.Constants;
using EmailClient.Interfaces;
using EmailClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Services
{
    public class SmtpEmailService : IEmailSendingService
    {
        public async Task SendEmailAsync(EmailConnectionInfo connectionInfo, ComposeEmailData emailData)
        {
            using (var smtpClient = new SmtpClient(AppConstants.Smtp.GMAIL_SERVER, AppConstants.Smtp.GMAIL_PORT))
            {
                smtpClient.EnableSsl = AppConstants.Smtp.USE_SSL;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(connectionInfo.Email, connectionInfo.Password);

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(connectionInfo.Email);
                    mailMessage.Subject = emailData.Subject;
                    mailMessage.Body = emailData.Body;
                    mailMessage.IsBodyHtml = false;

                    if (!string.IsNullOrEmpty(emailData.ReplyTo))
                    {
                        mailMessage.ReplyToList.Add(new MailAddress(emailData.ReplyTo));
                    }

                    mailMessage.To.Add(emailData.To);

                    foreach (var attachmentPath in emailData.Attachments)
                    {
                        if (File.Exists(attachmentPath))
                        {
                            var attachment = new Attachment(attachmentPath);
                            mailMessage.Attachments.Add(attachment);
                        }
                    }

                    await Task.Run(() => smtpClient.Send(mailMessage));
                }
            }
        }
    }
}
