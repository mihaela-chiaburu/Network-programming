using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Interfaces
{
    public interface IDnsResolver
    {
        Task<string[]> GetIpAddressesByHostAsync(string hostname);
        Task<string[]> GetIpAddressesByHostAsync(string hostname, string customDnsServer);
        Task<string[]> GetHostnamesByIpAsync(string ipAddress);
    }
}
