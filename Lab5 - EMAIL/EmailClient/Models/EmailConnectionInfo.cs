using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Models
{
    public class EmailConnectionInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImapServer { get; set; }
        public int ImapPort { get; set; }
        public string Pop3Server { get; set; }
        public int Pop3Port { get; set; }
        public bool UseImap { get; set; }
    }
}
