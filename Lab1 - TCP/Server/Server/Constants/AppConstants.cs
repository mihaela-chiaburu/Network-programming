using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Constants
{
    public static class AppConstants
    {
        public static class Network
        {
            public const int SERVER_PORT = 65432;
            public const int BUFFER_SIZE = 1024;
            public const string SERVER_IP = "0.0.0.0"; // all interfaces
        }

        public static class UI
        {
            public const string PLACEHOLDER_TEXT = "Type your message here...";
            public const int MESSAGE_FONT_SIZE = 18;
            public const int MAX_MESSAGE_WIDTH = 400;
            public const string SERVER_PREFIX = "Server: ";
        }

        public static class Messages
        {
            public const string SERVER_STARTED = "Server started on port ";
            public const string SERVER_STOPPED = "Server stopped";
            public const string SERVER_START_ERROR = "Error starting server: ";
            public const string CLIENT_JOINED = " joined the chat.";
            public const string CLIENT_DISCONNECTED = "A client has disconnected unexpectedly.";
            public const string CLIENT_LEFT = " left the chat.";
            public const string BROADCAST_ERROR = "Error broadcasting message: ";
            public const string CLIENT_HANDLE_ERROR = "Error handling client: ";
        }

        public static class Titles
        {
            public const string SERVER_ERROR = "Server Error";
            public const string CONNECTION_ERROR = "Connection Error";
        }
    }
}
