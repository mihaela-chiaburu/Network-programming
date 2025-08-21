using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Constants
{
    public static class AppConstants
    {
        public static class Network
        {
            public const int DNS_PORT = 53;
            public const int DEFAULT_TIMEOUT_MS = 2000;
            public const int PING_TIMEOUT_MS = 2000;
            public const string DEFAULT_DNS_SERVER = "System Default";
        }

        public static class UI
        {
            public const string RESOLVE_COMMAND_PREFIX = "resolve ";
            public const string OUTPUT_SEPARATOR = "\n";
            public const string RESULT_SEPARATOR = "\n";
        }

        public static class Messages
        {
            public const string ENTER_DNS_ADDRESS = "Introduceți o adresă DNS.";
            public const string DNS_SERVER_SET = "Server DNS setat la: ";
            public const string DNS_SERVER_UNREACHABLE = "Serverul DNS nu este accesibil.";
            public const string INVALID_DNS_ADDRESS = "Adresă DNS invalidă.";
            public const string DOMAINS_ASSOCIATED_WITH = "Domenii asociate cu ";
            public const string NO_DOMAINS_FOUND = "Nu s-au găsit domenii pentru IP-ul ";
            public const string IP_ADDRESSES_ASSOCIATED_WITH = "Adrese IP asociate cu ";
            public const string NO_IP_ADDRESSES_FOUND = "Nu s-au găsit adrese IP pentru domeniul ";
            public const string ERROR_RESOLVING_DOMAIN = "Eroare la rezolvarea domeniului: ";
            public const string ERROR_RESOLVING_IP = "Eroare la rezolvarea IP-ului: ";
            public const string DNS_ERROR = "Eroare DNS: ";
            public const string CUSTOM_DNS_ERROR = "Eroare la rezolvarea DNS custom: ";
            public const string GENERAL_ERROR = "Eroare: ";

            public const string DNS_SERVICE_INITIALIZED = "Serviciul DNS a fost inițializat";
            public const string USING_SYSTEM_DNS = "Se folosește DNS-ul sistemului";
            public const string USING_CUSTOM_DNS = "Se folosește DNS custom: ";
            public const string DNS_QUERY_STARTED = "Începe interogarea DNS...";
            public const string DNS_QUERY_COMPLETED = "Interogarea DNS s-a finalizat";
        }

        public static class Validation
        {
            public const int MIN_HOSTNAME_LENGTH = 1;
            public const int MAX_HOSTNAME_LENGTH = 253;
            public const string HOSTNAME_PATTERN = @"^[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*$";
        }
    }
}
