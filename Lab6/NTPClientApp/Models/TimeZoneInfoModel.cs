using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Models
{
    public class TimeZoneInfoModel
    {
        public string ZoneIdentifier { get; set; }
        public int OffsetHours { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public TimeZoneInfoModel()
        {
            IsValid = true;
        }

        public static TimeZoneInfoModel CreateInvalid(string errorMessage)
        {
            return new TimeZoneInfoModel
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }

        public static TimeZoneInfoModel CreateValid(string zoneIdentifier, int offsetHours)
        {
            return new TimeZoneInfoModel
            {
                ZoneIdentifier = zoneIdentifier,
                OffsetHours = offsetHours,
                IsValid = true
            };
        }
    }
}
