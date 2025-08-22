using EmailClient.Constants;
using EmailClient.Interfaces;
using EmailClient.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Services
{
    public class ImapEmailService : IEmailRetrievalService
    {
        public async Task<List<MimeMessage>> LoadEmailsAsync(EmailConnectionInfo connectionInfo)
        {
            var emailMessages = new List<MimeMessage>();

            using (var client = new ImapClient())
            {
                await client.ConnectAsync(connectionInfo.ImapServer, connectionInfo.ImapPort, true);
                await client.AuthenticateAsync(connectionInfo.Email, connectionInfo.Password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                var count = Math.Min(inbox.Count, AppConstants.Email.MAX_EMAILS_TO_LOAD);
                var uids = await inbox.SearchAsync(SearchQuery.All);
                var latestUids = uids.TakeLast(count).Reverse().ToList();

                foreach (var uid in latestUids)
                {
                    var message = await inbox.GetMessageAsync(uid);
                    emailMessages.Add(message);
                }

                await client.DisconnectAsync(true);
            }

            return emailMessages;
        }
    }
}
