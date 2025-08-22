using NTPClientApp.Interfaces;
using NTPClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Services
{
    public class TimeService : ITimeService
    {
        private readonly INtpClient _ntpClient;
        private readonly ITimeZoneService _timeZoneService;

        public TimeService(INtpClient ntpClient, ITimeZoneService timeZoneService)
        {
            _ntpClient = ntpClient;
            _timeZoneService = timeZoneService;
        }

        public async Task<TimeResult> GetTimeForZoneAsync(string zoneString, string ntpServer = null)
        {
            var timeZone = _timeZoneService.ParseTimeZone(zoneString);
            if (!timeZone.IsValid)
                return TimeResult.CreateError(timeZone.ErrorMessage);

            var ntpResponse = await _ntpClient.GetNetworkTimeAsync(ntpServer);
            if (!ntpResponse.IsSuccessful)
                return TimeResult.CreateError(ntpResponse.ErrorMessage);

            var localTime = _timeZoneService.ConvertToLocalTime(ntpResponse.UtcTime, timeZone);

            return TimeResult.CreateSuccess(
                ntpResponse.UtcTime,
                localTime,
                timeZone,
                ntpResponse.ServerName,
                ntpResponse.RoundTripTime);
        }

        public async Task<TimeResult> GetUtcTimeAsync(string ntpServer = null)
        {
            var ntpResponse = await _ntpClient.GetNetworkTimeAsync(ntpServer);
            if (!ntpResponse.IsSuccessful)
                return TimeResult.CreateError(ntpResponse.ErrorMessage);

            var utcTimeZone = TimeZoneInfoModel.CreateValid("GMT", 0);

            return TimeResult.CreateSuccess(
                ntpResponse.UtcTime,
                ntpResponse.UtcTime,
                utcTimeZone,
                ntpResponse.ServerName,
                ntpResponse.RoundTripTime);
        }
    }
}
