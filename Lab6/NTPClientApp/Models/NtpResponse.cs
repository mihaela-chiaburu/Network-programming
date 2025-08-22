using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Models
{
    public class NtpResponse
    {
        public DateTime UtcTime { get; set; }
        public TimeSpan RoundTripTime { get; set; }
        public string ServerName { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public NtpResponse()
        {
            IsSuccessful = true;
        }

        public static NtpResponse CreateError(string errorMessage)
        {
            return new NtpResponse
            {
                IsSuccessful = false,
                ErrorMessage = errorMessage
            };
        }

        public static NtpResponse CreateSuccess(DateTime utcTime, string serverName, TimeSpan roundTripTime)
        {
            return new NtpResponse
            {
                UtcTime = utcTime,
                ServerName = serverName,
                RoundTripTime = roundTripTime,
                IsSuccessful = true
            };
        }
    }
}
