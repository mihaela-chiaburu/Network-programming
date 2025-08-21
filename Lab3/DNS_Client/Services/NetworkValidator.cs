using DNS_Client.Constants;
using DNS_Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DNS_Client.Services
{
    public class NetworkValidator : INetworkValidator
    {
        public bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            return IPAddress.TryParse(ipAddress, out _);
        }

        public bool IsValidHostname(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
                return false;

            if (hostname.Length < AppConstants.Validation.MIN_HOSTNAME_LENGTH ||
                hostname.Length > AppConstants.Validation.MAX_HOSTNAME_LENGTH)
                return false;

            return Regex.IsMatch(hostname, AppConstants.Validation.HOSTNAME_PATTERN);
        }

        public async Task<bool> IsDnsServerReachableAsync(string dnsServer, int timeoutMs = AppConstants.Network.DEFAULT_TIMEOUT_MS)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(dnsServer, timeoutMs);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
