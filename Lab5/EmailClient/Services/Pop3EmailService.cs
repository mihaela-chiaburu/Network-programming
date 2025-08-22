using EmailClient.Interfaces;
using EmailClient.Models;
using MailKit.Net.Pop3;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Services
{
    public class Pop3EmailService : IEmailRetrievalService
    {
        public async Task<List<MimeMessage>> LoadEmailsAsync(EmailConnectionInfo connectionInfo)
        {
            var emailMessages = new List<MimeMessage>();

            using (var client = new Pop3Client())
            {
                await client.ConnectAsync(connectionInfo.Pop3Server, connectionInfo.Pop3Port, true);
                await client.AuthenticateAsync(connectionInfo.Email, connectionInfo.Password);

                for (int i = 0; i < client.Count; i++)
                {
                    var message = await client.GetMessageAsync(i);
                    emailMessages.Add(message);
                }

                await client.DisconnectAsync(true);
            }

            return emailMessages;
        }
    }
}
