using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Constants
{
    public static class AppConstants
    {
        public static class Network
        {
            public const int UDP_PORT = 9000;
            public const string BROADCAST_IP = "255.255.255.255";
            public const string BASE_IP_RANGE = "127.0.0.";
            public const int MIN_IP_OCTET = 2;
            public const int MAX_IP_OCTET = 255;
            public const string PRIVATE_MESSAGE_PREFIX = "PRIVATE_FROM:";
            public const string PRIVATE_COMMAND_PREFIX = "private ";
        }

        public static class UI
        {
            public const string PLACEHOLDER_TEXT = "Type your message here...";
            public const int MESSAGE_FONT_SIZE = 18;
            public const int MAX_MESSAGE_WIDTH = 400;
            public const string USER_PREFIX = "User";
            public const string ME_PREFIX = "Me: ";
            public const string PRIVATE_TO_PREFIX = "Me (to ";
            public const string PRIVATE_FROM_PREFIX = "private message from ";
            public const int USERNAME_MIN = 1000;
            public const int USERNAME_MAX = 9999;
        }

        public static class Messages
        {
            public const string INITIALIZATION_ERROR = "Error initializing UDP client: ";
            public const string SEND_ERROR = "Error sending message: ";
            public const string RECEIVE_ERROR = "Error receiving message: ";
            public const string INVALID_IP_ERROR = "Invalid IP address format.";
            public const string PRIVATE_MESSAGE_FORMAT_ERROR = "Invalid private message format. Use: private [IP]:[message]";
            public const string CLIENT_INITIALIZED = "UDP client initialized successfully";
            public const string CLIENT_DISPOSED = "UDP client disposed";
        }

        public static class Titles
        {
            public const string ERROR_TITLE = "Error";
            public const string WARNING_TITLE = "Warning";
            public const string INFO_TITLE = "Information";
        }

        public static class Format
        {
            public const string WINDOW_TITLE = "UDP Chat Client - {0} ({1})";
            public const char PRIVATE_MESSAGE_SEPARATOR = ':';
        }
    }
}
