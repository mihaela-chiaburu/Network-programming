using NTPClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Interfaces
{
    public interface INtpClient
    {
        Task<NtpResponse> GetNetworkTimeAsync(string ntpServer = null);
        Task<NtpResponse> GetNetworkTimeAsync(string ntpServer, int timeoutMs);
    }
}
