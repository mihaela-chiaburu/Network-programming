using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Models
{
    public class ComposeEmailData
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ReplyTo { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }
}
