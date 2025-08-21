using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ParsedMessage
    {
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public string SenderInfo { get; set; }
        public string TargetInfo { get; set; }
        public bool IsFromCurrentClient { get; set; }

        public ParsedMessage()
        {
            Content = string.Empty;
            SenderInfo = string.Empty;
            TargetInfo = string.Empty;
        }
    }
}
