using Client.Constants;
using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class NetworkUtils : INetworkUtils
    {
        private readonly Random _random;

        public NetworkUtils()
        {
            _random = new Random();
        }

        public string GenerateRandomIP()
        {
            int lastOctet = _random.Next(AppConstants.Network.MIN_IP_OCTET, AppConstants.Network.MAX_IP_OCTET);
            return $"{AppConstants.Network.BASE_IP_RANGE}{lastOctet}";
        }

        public string GenerateRandomUsername()
        {
            int userNumber = _random.Next(AppConstants.UI.USERNAME_MIN, AppConstants.UI.USERNAME_MAX);
            return $"{AppConstants.UI.USER_PREFIX}{userNumber}";
        }

        public bool IsValidIPAddress(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;

            return IPAddress.TryParse(ip, out _);
        }
    }
}
