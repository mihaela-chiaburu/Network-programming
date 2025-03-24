using DnsClient;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace DNS_Client
{
    public partial class MainWindow : Window
    {
        private string customDnsServer = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResolveButton_Click(object sender, RoutedEventArgs e)
        {
            string input = ResolveInputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            string query = input.StartsWith("resolve ") ? input.Substring(8) : input;
            ResolveDomainOrIp(query);
        }

        private void UseDnsButton_Click(object sender, RoutedEventArgs e)
        {
            string dns = DnsServerTextBox.Text.Trim();

            if (string.IsNullOrEmpty(dns))
            {
                AppendOutput("Introduceți o adresă DNS.");
                return;
            }

            ChangeDnsServer(dns);
        }


        private void ResolveDomainOrIp(string query)
        {
            try
            {
                if (IPAddress.TryParse(query, out IPAddress address))
                {
                    string[] domains = GetHostByAddress(query);
                    string result = domains.Length > 0 ? $"Domenii asociate cu {query}:\n{string.Join("\n", domains)}" : $"Nu s-au găsit domenii pentru IP-ul {query}.";
                    AppendOutput(result);
                }
                else
                {
                    string[] addresses = GetIpByHost(query);
                    string result = addresses.Length > 0 ? $"Adrese IP asociate cu {query}:\n{string.Join("\n", addresses)}" : $"Nu s-au găsit adrese IP pentru domeniul {query}.";
                    AppendOutput(result);
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Eroare: {ex.Message}");
            }
        }


        private bool IsDnsServerReachable(string dnsServer)
        {
            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = ping.Send(dnsServer, 2000);
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ChangeDnsServer(string dns)
        {
            if (IPAddress.TryParse(dns, out _))
            {
                if (IsDnsServerReachable(dns))
                {
                    customDnsServer = dns;
                    AppendOutput($"Server DNS setat la: {dns}");
                }
                else
                {
                    AppendOutput("Serverul DNS nu este accesibil.");
                }
            }
            else
            {
                AppendOutput("Adresă DNS invalidă.");
            }
        }

        private string[] GetIpByHost(string host)
        {
            try
            {
                if (string.IsNullOrEmpty(customDnsServer))
                {
                    return Dns.GetHostAddresses(host).Select(ip => ip.ToString()).ToArray();
                }
                else
                {
                    return GetHostAddressesUsingCustomDns(host, customDnsServer);
                }
            }
            catch (Exception ex)
            {
                return new string[] { $"Eroare la rezolvarea domeniului: {ex.Message}" };
            }
        }

        private string[] GetHostByAddress(string ip)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                return hostEntry.Aliases.Concat(new[] { hostEntry.HostName }).ToArray();
            }
            catch (Exception ex)
            {
                AppendOutput($"Eroare la rezolvarea IP-ului: {ex.Message}");
                return new string[0];
            }
        }

        private static string[] GetHostAddressesUsingCustomDns(string host, string dnsServer)
        {
            try
            {
                var endpoint = new IPEndPoint(IPAddress.Parse(dnsServer), 53);
                var client = new LookupClient(endpoint);

                var result = client.Query(host, QueryType.A);

                if (result.HasError)
                {
                    return new string[] { $"Eroare DNS: {result.ErrorMessage}" };
                }

                return result.Answers.ARecords().Select(record => record.Address.ToString()).ToArray();
            }
            catch (Exception ex)
            {
                return new string[] { $"Eroare la rezolvarea DNS custom: {ex.Message}" };
            }
        }

        private void AppendOutput(string text)
        {
            OutputTextBox.AppendText($"{text}\n");
            OutputTextBox.ScrollToEnd();
        }
    }
}
