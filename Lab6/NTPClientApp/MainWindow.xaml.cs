using NTPClientApp.Constants;
using NTPClientApp.Interfaces;
using NTPClientApp.Services;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace NTPClientApp
{
    public partial class MainWindow : Window
    {
        private ITimeService _timeService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            var packetService = new NtpPacketService();
            var ntpClient = new NtpClient(packetService);
            var timeZoneService = new TimeZoneService();
            _timeService = new TimeService(ntpClient, timeZoneService);
        }

        private async void OnGetTimeClick(object sender, RoutedEventArgs e)
        {
            string zone = ZoneTextBox.Text?.Trim();

            if (string.IsNullOrEmpty(zone) || !zone.StartsWith(AppConstants.TimeZone.GMT_PREFIX))
            {
                DisplayError(AppConstants.Messages.ENTER_VALID_ZONE);
                return;
            }

            await GetAndDisplayTime(zone);
        }

        private async Task GetAndDisplayTime(string zone)
        {
            try
            {
                UpdateStatus(AppConstants.Messages.CONNECTING_TO_SERVER);
                DisableControls();

                var result = await _timeService.GetTimeForZoneAsync(zone);

                if (result.IsSuccessful)
                {
                    DisplayTimeResult(result);
                    UpdateStatus(AppConstants.Messages.TIME_UPDATED);
                }
                else
                {
                    DisplayError(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
            finally
            {
                EnableControls();
            }
        }

        private void DisplayTimeResult(Models.TimeResult result)
        {
            var message = string.Format(
                AppConstants.Messages.EXACT_TIME_FORMAT,
                result.TimeZone.ZoneIdentifier,
                result.LocalTime);

            TimeLabel.Content = message;

            if (result.RoundTripTime.TotalMilliseconds > 0)
            {
                var additionalInfo = $"\nServer: {result.ServerName}\nRound-trip: {result.RoundTripTime.TotalMilliseconds:F0}ms";
            }
        }

        private void DisplayError(string errorMessage)
        {
            TimeLabel.Content = string.Format(AppConstants.Messages.ERROR_FORMAT, errorMessage);
        }

        private void UpdateStatus(string message)
        {
            // --
        }

        private void DisableControls()
        {
            ZoneTextBox.IsEnabled = false;
            GetTimeButton.IsEnabled = false;
        }

        private void EnableControls()
        {
            ZoneTextBox.IsEnabled = true;
            GetTimeButton.IsEnabled = true;
        }

        private async void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            string zone = ZoneTextBox.Text?.Trim();
            if (!string.IsNullOrEmpty(zone))
            {
                await GetAndDisplayTime(zone);
            }
        }

        private void OnZoneTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnGetTimeClick(sender, e);
            }
        }

        private async void OnGMTClick(object sender, RoutedEventArgs e)
        {
            ZoneTextBox.Text = "GMT";
            await GetAndDisplayTime("GMT");
        }

        private async void OnGMTPlus2Click(object sender, RoutedEventArgs e)
        {
            ZoneTextBox.Text = "GMT+2";
            await GetAndDisplayTime("GMT+2");
        }

        private async void OnGMTMinus5Click(object sender, RoutedEventArgs e)
        {
            ZoneTextBox.Text = "GMT-5";
            await GetAndDisplayTime("GMT-5");
        }
    }
}
