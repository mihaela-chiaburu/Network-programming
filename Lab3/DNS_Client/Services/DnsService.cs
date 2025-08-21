using DNS_Client.Constants;
using DNS_Client.Interfaces;
using DNS_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_Client.Services
{
    public class DnsService : IDnsService
    {
        private readonly IDnsResolver _dnsResolver;
        private readonly INetworkValidator _networkValidator;
        private DnsServerInfo _customDnsServer;

        public event Action<string> OutputReceived;
        public event Action<string> StatusChanged;

        public string CurrentDnsServer => _customDnsServer?.Address ?? AppConstants.Network.DEFAULT_DNS_SERVER;
        public bool IsUsingCustomDns => _customDnsServer != null;

        public DnsService(IDnsResolver dnsResolver, INetworkValidator networkValidator)
        {
            _dnsResolver = dnsResolver ?? throw new ArgumentNullException(nameof(dnsResolver));
            _networkValidator = networkValidator ?? throw new ArgumentNullException(nameof(networkValidator));

            StatusChanged?.Invoke(AppConstants.Messages.DNS_SERVICE_INITIALIZED);
        }

        public async Task<DnsResult> ResolveAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return DnsResult.Error("Query cannot be empty", DnsQueryType.Forward, query);
            }

            StatusChanged?.Invoke(AppConstants.Messages.DNS_QUERY_STARTED);

            try
            {
                string cleanQuery = query.StartsWith(AppConstants.UI.RESOLVE_COMMAND_PREFIX)
                    ? query.Substring(AppConstants.UI.RESOLVE_COMMAND_PREFIX.Length).Trim()
                    : query.Trim();

                DnsResult result;

                if (_networkValidator.IsValidIpAddress(cleanQuery))
                {
                    result = await PerformReverseLookupAsync(cleanQuery);
                }
                else
                {
                    result = await PerformForwardLookupAsync(cleanQuery);
                }

                StatusChanged?.Invoke(AppConstants.Messages.DNS_QUERY_COMPLETED);
                return result;
            }
            catch (Exception ex)
            {
                var errorResult = DnsResult.Error(AppConstants.Messages.GENERAL_ERROR + ex.Message,
                    DnsQueryType.Forward, query);
                return errorResult;
            }
        }

        private async Task<DnsResult> PerformForwardLookupAsync(string hostname)
        {
            try
            {
                string[] addresses;

                if (IsUsingCustomDns)
                {
                    addresses = await _dnsResolver.GetIpAddressesByHostAsync(hostname, _customDnsServer.Address);
                }
                else
                {
                    addresses = await _dnsResolver.GetIpAddressesByHostAsync(hostname);
                }

                if (addresses.Length > 0)
                {
                    return DnsResult.Success(addresses, DnsQueryType.Forward, hostname, CurrentDnsServer);
                }
                else
                {
                    return DnsResult.Error(AppConstants.Messages.NO_IP_ADDRESSES_FOUND + hostname + ".",
                        DnsQueryType.Forward, hostname);
                }
            }
            catch (DnsException ex)
            {
                return DnsResult.Error(ex.Message, DnsQueryType.Forward, hostname);
            }
        }

        private async Task<DnsResult> PerformReverseLookupAsync(string ipAddress)
        {
            try
            {
                string[] hostnames = await _dnsResolver.GetHostnamesByIpAsync(ipAddress);

                if (hostnames.Length > 0)
                {
                    return DnsResult.Success(hostnames, DnsQueryType.Reverse, ipAddress, CurrentDnsServer);
                }
                else
                {
                    return DnsResult.Error(AppConstants.Messages.NO_DOMAINS_FOUND + ipAddress + ".",
                        DnsQueryType.Reverse, ipAddress);
                }
            }
            catch (DnsException ex)
            {
                return DnsResult.Error(ex.Message, DnsQueryType.Reverse, ipAddress);
            }
        }

        public async Task<bool> SetCustomDnsServerAsync(string dnsServer)
        {
            if (string.IsNullOrWhiteSpace(dnsServer))
            {
                OutputReceived?.Invoke(AppConstants.Messages.ENTER_DNS_ADDRESS);
                return false;
            }

            if (!_networkValidator.IsValidIpAddress(dnsServer))
            {
                OutputReceived?.Invoke(AppConstants.Messages.INVALID_DNS_ADDRESS);
                return false;
            }

            bool isReachable = await _networkValidator.IsDnsServerReachableAsync(dnsServer);
            if (!isReachable)
            {
                OutputReceived?.Invoke(AppConstants.Messages.DNS_SERVER_UNREACHABLE);
                return false;
            }

            _customDnsServer = new DnsServerInfo(dnsServer)
            {
                IsReachable = true
            };

            OutputReceived?.Invoke(AppConstants.Messages.DNS_SERVER_SET + dnsServer);
            StatusChanged?.Invoke(AppConstants.Messages.USING_CUSTOM_DNS + dnsServer);
            return true;
        }

        public void ClearCustomDnsServer()
        {
            _customDnsServer = null;
            StatusChanged?.Invoke(AppConstants.Messages.USING_SYSTEM_DNS);
        }
    }
}
