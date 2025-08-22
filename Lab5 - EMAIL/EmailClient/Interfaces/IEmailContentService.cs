using EmailClient.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Interfaces
{
    public interface IEmailContentService
    {
        EmailDisplayInfo PrepareEmailForDisplay(MimeMessage message);
        ComposeEmailData PrepareReplyData(MimeMessage originalMessage, string userEmail);
    }
}
