using EmailClient.Interfaces;
using EmailClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Services
{
    public class EmailServiceFactory
    {
        public static IEmailRetrievalService CreateEmailRetrievalService(EmailConnectionInfo connectionInfo)
        {
            return connectionInfo.UseImap ? new ImapEmailService() : new Pop3EmailService();
        }
    }
}
