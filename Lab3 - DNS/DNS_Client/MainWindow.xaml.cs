using DNS_Client.Constants;
using DNS_Client.Interfaces;
using DNS_Client.Models;
using DNS_Client.Services;
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
        private readonly IDnsService _dnsService;
        private readonly IOutputService _outputService;

        public MainWindow() : this(
            new DnsService(new DnsResolver(), new NetworkValidator()),
            new OutputService())
        {
        }

        public MainWindow(IDnsService dnsService, IOutputService outputService)
        {
            InitializeComponent();

            _dnsService = dnsService ?? throw new ArgumentNullException(nameof(dnsService));
            _outputService = outputService ?? throw new ArgumentNullException(nameof(outputService));

            InitializeServices();
        }

        private void InitializeServices()
        {
            _dnsService.OutputReceived += OnOutputReceived;
            _dnsService.StatusChanged += OnStatusChanged;
        }

        private void OnOutputReceived(string output)
        {
            _outputService.AppendOutput(output, OutputTextBox);
        }

        private void OnStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                Console.WriteLine($"DNS Status: {status}");
            });
        }

        private async void ResolveButton_Click(object sender, RoutedEventArgs e)
        {
            string input = ResolveInputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                _outputService.AppendError("Please enter a domain or IP address to resolve.", OutputTextBox);
                return;
            }

            try
            {
                ResolveButton.IsEnabled = false;

                var result = await _dnsService.ResolveAsync(input);
                DisplayDnsResult(result);
            }
            catch (Exception ex)
            {
                _outputService.AppendError($"{AppConstants.Messages.GENERAL_ERROR}{ex.Message}", OutputTextBox);
            }
            finally
            {
                ResolveButton.IsEnabled = true;
            }
        }

        private async void UseDnsButton_Click(object sender, RoutedEventArgs e)
        {
            string dnsServer = DnsServerTextBox.Text.Trim();

            try
            {
                UseDnsButton.IsEnabled = false;

                bool success = await _dnsService.SetCustomDnsServerAsync(dnsServer);

                if (success)
                {
                    DnsServerTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                _outputService.AppendError($"{AppConstants.Messages.GENERAL_ERROR}{ex.Message}", OutputTextBox);
            }
            finally
            {
                UseDnsButton.IsEnabled = true;
            }
        }

        private void DisplayDnsResult(DnsResult result)
        {
            if (result.IsSuccess && result.Results.Length > 0)
            {
                string header = result.QueryType == DnsQueryType.Forward
                    ? $"{AppConstants.Messages.IP_ADDRESSES_ASSOCIATED_WITH}{result.Query}:"
                    : $"{AppConstants.Messages.DOMAINS_ASSOCIATED_WITH}{result.Query}:";

                _outputService.AppendSuccess(header, OutputTextBox);

                foreach (string resultItem in result.Results)
                {
                    _outputService.AppendOutput($"  • {resultItem}", OutputTextBox);
                }

                if (_dnsService.IsUsingCustomDns)
                {
                    _outputService.AppendInfo($"(Using DNS server: {result.DnsServerUsed})", OutputTextBox);
                }
            }
            else
            {
                string errorMessage = !string.IsNullOrEmpty(result.ErrorMessage)
                    ? result.ErrorMessage
                    : (result.QueryType == DnsQueryType.Forward
                        ? $"{AppConstants.Messages.NO_IP_ADDRESSES_FOUND}{result.Query}."
                        : $"{AppConstants.Messages.NO_DOMAINS_FOUND}{result.Query}.");

                _outputService.AppendError(errorMessage, OutputTextBox);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _outputService.Clear(OutputTextBox);
        }

        private void ResetDnsButton_Click(object sender, RoutedEventArgs e)
        {
            _dnsService.ClearCustomDnsServer();
            _outputService.AppendInfo("DNS server reset to system default.", OutputTextBox);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
