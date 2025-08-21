using DNS_Client.Constants;
using DNS_Client.Interfaces;
using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Services
{
    public class DnsResolver : IDnsResolver
    {
        public async Task<string[]> GetIpAddressesByHostAsync(string hostname)
        {
            try
            {
                var hostEntry = await Dns.GetHostEntryAsync(hostname);
                return hostEntry.AddressList.Select(ip => ip.ToString()).ToArray();
            }
            catch (Exception ex)
            {
                throw new DnsException(AppConstants.Messages.ERROR_RESOLVING_DOMAIN + ex.Message);
            }
        }

        public async Task<string[]> GetIpAddressesByHostAsync(string hostname, string customDnsServer)
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(customDnsServer), AppConstants.Network.DNS_PORT);
                var client = new LookupClient(endpoint);

                var result = await client.QueryAsync(hostname, QueryType.A);

                if (result.HasError)
                {
                    throw new DnsException(AppConstants.Messages.DNS_ERROR + result.ErrorMessage);
                }

                return result.Answers.ARecords().Select(record => record.Address.ToString()).ToArray();
            }
            catch (DnsException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new DnsException(AppConstants.Messages.CUSTOM_DNS_ERROR + ex.Message);
            }
        }

        public async Task<string[]> GetHostnamesByIpAsync(string ipAddress)
        {
            try
            {
                var hostEntry = await Dns.GetHostEntryAsync(ipAddress);
                return hostEntry.Aliases.Concat(new[] { hostEntry.HostName })
                                       .Where(name => !string.IsNullOrEmpty(name))
                                       .ToArray();
            }
            catch (Exception ex)
            {
                throw new DnsException(AppConstants.Messages.ERROR_RESOLVING_IP + ex.Message);
            }
        }
    }

    public class DnsException : Exception
    {
        public DnsException(string message) : base(message) { }
        public DnsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
