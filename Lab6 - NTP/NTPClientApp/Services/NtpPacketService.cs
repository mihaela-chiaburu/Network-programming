using NTPClientApp.Constants;
using NTPClientApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Services
{
    public class NtpPacketService : INtpPacketService
    {
        public byte[] CreateNtpPacket()
        {
            var ntpData = new byte[AppConstants.NtpConfiguration.NTP_PACKET_SIZE];
            ntpData[0] = AppConstants.NtpConfiguration.NTP_MODE_CLIENT;
            return ntpData;
        }

        public DateTime ParseNtpTimestamp(byte[] ntpData)
        {
            if (!ValidateNtpResponse(ntpData))
                throw new ArgumentException(AppConstants.Messages.INCOMPLETE_NTP_RESPONSE);

            uint seconds = BitConverter.ToUInt32(ntpData, AppConstants.NtpConfiguration.NTP_TIMESTAMP_OFFSET);

            if (BitConverter.IsLittleEndian)
            {
                seconds = (uint)IPAddress.NetworkToHostOrder((int)seconds);
            }

            return AppConstants.NtpEpoch.EPOCH_START.AddSeconds(seconds).ToUniversalTime();
        }

        public bool ValidateNtpResponse(byte[] ntpData)
        {
            return ntpData != null && ntpData.Length >= AppConstants.NtpConfiguration.NTP_PACKET_SIZE;
        }
    }
}
