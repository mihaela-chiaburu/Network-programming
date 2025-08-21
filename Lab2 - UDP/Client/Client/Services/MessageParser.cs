using Client.Constants;
using Client.Interfaces;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class MessageParser : IMessageParser
    {
        public ParsedMessage ParseMessage(string rawMessage, string currentClientInfo)
        {
            var parsed = new ParsedMessage();

            if (string.IsNullOrWhiteSpace(rawMessage))
            {
                parsed.Type = MessageType.System;
                parsed.Content = "Empty message received";
                return parsed;
            }

            if (rawMessage.StartsWith(AppConstants.Network.PRIVATE_MESSAGE_PREFIX))
            {
                return ParsePrivateMessage(rawMessage, currentClientInfo);
            }

            if (rawMessage.StartsWith($"{currentClientInfo}:"))
            {
                parsed.Type = MessageType.Broadcast;
                parsed.IsFromCurrentClient = true;
                parsed.Content = AppConstants.UI.ME_PREFIX + rawMessage.Substring($"{currentClientInfo}:".Length).Trim();
                parsed.SenderInfo = currentClientInfo;
            }
            else
            {
                parsed.Type = MessageType.Broadcast;
                parsed.IsFromCurrentClient = false;
                parsed.Content = rawMessage;

                var colonIndex = rawMessage.IndexOf(':');
                if (colonIndex > 0)
                {
                    parsed.SenderInfo = rawMessage.Substring(0, colonIndex).Trim();
                }
            }

            return parsed;
        }

        private ParsedMessage ParsePrivateMessage(string rawMessage, string currentClientInfo)
        {
            var parsed = new ParsedMessage();

            var content = rawMessage.Substring(AppConstants.Network.PRIVATE_MESSAGE_PREFIX.Length);
            var parts = content.Split(new[] { ':' }, 2);

            if (parts.Length == 2)
            {
                string senderInfo = parts[0].Trim();
                string privateMessage = parts[1].Trim();

                if (senderInfo == currentClientInfo)
                {
                    parsed.Type = MessageType.PrivateSent;
                    parsed.IsFromCurrentClient = true;
                    parsed.Content = AppConstants.UI.ME_PREFIX + privateMessage;
                    parsed.SenderInfo = currentClientInfo;
                }
                else
                {
                    parsed.Type = MessageType.PrivateReceived;
                    parsed.IsFromCurrentClient = false;
                    parsed.Content = $"{AppConstants.UI.PRIVATE_FROM_PREFIX}{senderInfo}: {privateMessage}";
                    parsed.SenderInfo = senderInfo;
                }
            }
            else
            {
                parsed.Type = MessageType.System;
                parsed.Content = "Malformed private message received";
            }

            return parsed;
        }

        public string FormatBroadcastMessage(string clientInfo, string message)
        {
            return $"{clientInfo}: {message}";
        }

        public string FormatPrivateMessage(string senderInfo, string message)
        {
            return $"{AppConstants.Network.PRIVATE_MESSAGE_PREFIX}{senderInfo}: {message}";
        }
    }
}
