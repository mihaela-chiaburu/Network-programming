using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace NTPClientApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnGetTimeClick(object sender, RoutedEventArgs e)
        {
            string zone = ZoneTextBox.Text.Trim();
            if (string.IsNullOrEmpty(zone) || !zone.StartsWith("GMT"))
            {
                TimeLabel.Content = "Introduceti o zona valida (ex: GMT+2)";
                return;
            }

            try
            {
                // ora de la un server NTP public
                DateTime utcNow = GetNetworkTime();
                int offset = GetOffsetFromZone(zone);
                DateTime localTime = utcNow.AddHours(offset);

                TimeLabel.Content = $"Ora exactă în {zone}: {localTime:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                TimeLabel.Content = $"Eroare: {ex.Message}";
            }
        }

        private DateTime GetNetworkTime()
        {
            string ntpServer = "time.windows.com";
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B; 

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            IPEndPoint endPoint = new IPEndPoint(addresses[0], 123); // NTP port 

            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.Client.ReceiveTimeout = 3000; 

                udpClient.Send(ntpData, ntpData.Length, endPoint);

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                ntpData = udpClient.Receive(ref remoteEndPoint);
            }

            if (ntpData.Length < 48)
                throw new Exception("Răspuns NTP incomplet");

            // Extract and convert timestamp
            uint seconds = BitConverter.ToUInt32(ntpData, 40);
            if (BitConverter.IsLittleEndian)
            {
                seconds = (uint)IPAddress.NetworkToHostOrder((int)seconds);
            }

            DateTime ntpEpoch = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return ntpEpoch.AddSeconds(seconds).ToUniversalTime();
        }

        private int GetOffsetFromZone(string zone)
        {
            // valoarea numerică din zona GMT
            if (zone.StartsWith("GMT+"))
                return int.Parse(zone.Substring(4));
            if (zone.StartsWith("GMT-"))
                return -int.Parse(zone.Substring(4));

            throw new ArgumentException("Formatul zonei este invalid.");
        }
    }
}
