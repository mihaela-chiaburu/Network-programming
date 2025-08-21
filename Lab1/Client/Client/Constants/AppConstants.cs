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
            public const string SERVER_IP = "127.0.0.1";
            public const int SERVER_PORT = 65432;
            public const int BUFFER_SIZE = 1024;
        }

        public static class UI
        {
            public const string PLACEHOLDER_TEXT = "Type your message here...";
            public const int MESSAGE_FONT_SIZE = 18;
            public const int MAX_MESSAGE_WIDTH = 400;
            public const string USER_PREFIX = "User";
            public const string ME_PREFIX = "Me: ";
        }

        public static class Messages
        {
            public const string CONNECTION_ERROR = "Error connecting to server: ";
            public const string NOT_CONNECTED = "Not connected to the server.";
            public const string SEND_ERROR = "Error sending message: ";
            public const string SERVER_DISCONNECTED = "Server has disconnected unexpectedly.";
            public const string CONNECTION_ERROR_TITLE = "Connection Error";
            public const string ERROR_TITLE = "Error";
        }
    }
}
