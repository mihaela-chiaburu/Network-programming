using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Models
{
    public class TimeResult
    {
        public DateTime LocalTime { get; set; }
        public DateTime UtcTime { get; set; }
        public TimeZoneInfoModel TimeZone { get; set; }
        public string ServerName { get; set; }
        public TimeSpan RoundTripTime { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public TimeResult()
        {
            IsSuccessful = true;
        }

        public static TimeResult CreateError(string errorMessage)
        {
            return new TimeResult
            {
                IsSuccessful = false,
                ErrorMessage = errorMessage
            };
        }

        public static TimeResult CreateSuccess(DateTime utcTime, DateTime localTime,
            TimeZoneInfoModel timeZone, string serverName, TimeSpan roundTripTime)
        {
            return new TimeResult
            {
                UtcTime = utcTime,
                LocalTime = localTime,
                TimeZone = timeZone,
                ServerName = serverName,
                RoundTripTime = roundTripTime,
                IsSuccessful = true
            };
        }
    }
}
