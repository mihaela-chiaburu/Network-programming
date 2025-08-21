using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IMessageParser
    {
        ParsedMessage ParseMessage(string rawMessage, string currentClientInfo);
        string FormatBroadcastMessage(string clientInfo, string message);
        string FormatPrivateMessage(string senderInfo, string message);
    }
}
