using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Models
{
    public class EmailDisplayInfo
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public string HtmlContent { get; set; }
        public string TextContent { get; set; }
        public bool HasAttachments { get; set; }
    }
}
