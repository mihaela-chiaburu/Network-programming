using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Models
{
    public class EmailMessage
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public MimeMessage Message { get; set; }

        public EmailMessage(MimeMessage mimeMessage)
        {
            Message = mimeMessage;
            From = mimeMessage.From.ToString();
            Subject = mimeMessage.Subject;
            Date = mimeMessage.Date.LocalDateTime.ToString("g");
        }
    }
}
