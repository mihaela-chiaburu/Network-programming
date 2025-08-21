using DNS_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Interfaces
{
    public interface IDnsService
    {
        event Action<string> OutputReceived;
        event Action<string> StatusChanged;

        Task<DnsResult> ResolveAsync(string query);
        Task<bool> SetCustomDnsServerAsync(string dnsServer);
        void ClearCustomDnsServer();

        string CurrentDnsServer { get; }
        bool IsUsingCustomDns { get; }
    }
}
