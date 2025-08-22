using EmailClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Interfaces
{
    public interface IEmailSendingService
    {
        Task SendEmailAsync(EmailConnectionInfo connectionInfo, ComposeEmailData emailData);
    }
}
