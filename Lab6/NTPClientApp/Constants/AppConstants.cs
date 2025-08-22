using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Constants
{
    public static class AppConstants
    {
        public static class NtpConfiguration
        {
            public const string DEFAULT_NTP_SERVER = "time.windows.com";
            public const int NTP_PORT = 123;
            public const int NTP_PACKET_SIZE = 48;
            public const byte NTP_MODE_CLIENT = 0x1B;
            public const int SOCKET_TIMEOUT_MS = 3000;
            public const int NTP_TIMESTAMP_OFFSET = 40;
        }

        public static class TimeZone
        {
            public const string GMT_PREFIX = "GMT";
            public const string GMT_PLUS_PREFIX = "GMT+";
            public const string GMT_MINUS_PREFIX = "GMT-";
            public const int GMT_PLUS_PREFIX_LENGTH = 4;
            public const int GMT_MINUS_PREFIX_LENGTH = 4;
        }

        public static class NtpEpoch
        {
            public static readonly DateTime EPOCH_START = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        public static class Messages
        {
            public const string ENTER_VALID_ZONE = "Introduceti o zona valida (ex: GMT+2)";
            public const string EXACT_TIME_FORMAT = "Ora exactă în {0}: {1:HH:mm:ss}";
            public const string ERROR_FORMAT = "Eroare: {0}";
            public const string INCOMPLETE_NTP_RESPONSE = "Răspuns NTP incomplet";
            public const string INVALID_ZONE_FORMAT = "Formatul zonei este invalid.";
            public const string CONNECTING_TO_SERVER = "Conectare la serverul NTP...";
            public const string TIME_UPDATED = "Ora actualizată cu succes";
        }

        public static class Validation
        {
            public const int MIN_TIMEZONE_OFFSET = -12;
            public const int MAX_TIMEZONE_OFFSET = 14;
        }
    }
}
