using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Interfaces
{
    public interface INetworkValidator
    {
        bool IsValidIpAddress(string ipAddress);
        bool IsValidHostname(string hostname);
        Task<bool> IsDnsServerReachableAsync(string dnsServer, int timeoutMs = 2000);
    }
}
