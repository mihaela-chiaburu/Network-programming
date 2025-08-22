using EmailClient.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Interfaces
{
    public interface IEmailRetrievalService
    {
        Task<List<MimeMessage>> LoadEmailsAsync(EmailConnectionInfo connectionInfo);
    }
}
