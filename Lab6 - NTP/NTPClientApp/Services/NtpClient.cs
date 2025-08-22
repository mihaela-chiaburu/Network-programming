using NTPClientApp.Constants;
using NTPClientApp.Interfaces;
using NTPClientApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Services
{
    public class NtpClient : INtpClient
    {
        private readonly INtpPacketService _packetService;

        public NtpClient(INtpPacketService packetService)
        {
            _packetService = packetService;
        }

        public async Task<NtpResponse> GetNetworkTimeAsync(string ntpServer = null)
        {
            return await GetNetworkTimeAsync(
                ntpServer ?? AppConstants.NtpConfiguration.DEFAULT_NTP_SERVER,
                AppConstants.NtpConfiguration.SOCKET_TIMEOUT_MS);
        }

        public async Task<NtpResponse> GetNetworkTimeAsync(string ntpServer, int timeoutMs)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var ntpData = _packetService.CreateNtpPacket();
                var addresses = await Dns.GetHostEntryAsync(ntpServer);
                var endPoint = new IPEndPoint(addresses.AddressList[0], AppConstants.NtpConfiguration.NTP_PORT);

                using (var udpClient = new UdpClient())
                {
                    udpClient.Client.ReceiveTimeout = timeoutMs;

                    await udpClient.SendAsync(ntpData, ntpData.Length, endPoint);
                    var result = await udpClient.ReceiveAsync();

                    stopwatch.Stop();

                    var utcTime = _packetService.ParseNtpTimestamp(result.Buffer);

                    return NtpResponse.CreateSuccess(utcTime, ntpServer, stopwatch.Elapsed);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return NtpResponse.CreateError(ex.Message);
            }
        }
    }
}
