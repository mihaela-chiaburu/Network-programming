using NTPClientApp.Constants;
using NTPClientApp.Interfaces;
using NTPClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Services
{
    public class TimeZoneService : ITimeZoneService
    {
        public TimeZoneInfoModel ParseTimeZone(string zoneString)
        {
            if (string.IsNullOrWhiteSpace(zoneString))
                return TimeZoneInfoModel.CreateInvalid(AppConstants.Messages.INVALID_ZONE_FORMAT);

            var trimmedZone = zoneString.Trim();

            if (!trimmedZone.StartsWith(AppConstants.TimeZone.GMT_PREFIX))
                return TimeZoneInfoModel.CreateInvalid(AppConstants.Messages.INVALID_ZONE_FORMAT);

            try
            {
                int offset;

                if (trimmedZone.StartsWith(AppConstants.TimeZone.GMT_PLUS_PREFIX))
                {
                    var offsetString = trimmedZone.Substring(AppConstants.TimeZone.GMT_PLUS_PREFIX_LENGTH);
                    offset = int.Parse(offsetString);
                }
                else if (trimmedZone.StartsWith(AppConstants.TimeZone.GMT_MINUS_PREFIX))
                {
                    var offsetString = trimmedZone.Substring(AppConstants.TimeZone.GMT_MINUS_PREFIX_LENGTH);
                    offset = -int.Parse(offsetString);
                }
                else if (trimmedZone.Equals(AppConstants.TimeZone.GMT_PREFIX))
                {
                    offset = 0;
                }
                else
                {
                    return TimeZoneInfoModel.CreateInvalid(AppConstants.Messages.INVALID_ZONE_FORMAT);
                }

                if (!IsValidTimeZoneOffset(offset))
                    return TimeZoneInfoModel.CreateInvalid($"Offset invalid: {offset}. Trebuie să fie între {AppConstants.Validation.MIN_TIMEZONE_OFFSET} și {AppConstants.Validation.MAX_TIMEZONE_OFFSET}");

                return TimeZoneInfoModel.CreateValid(trimmedZone, offset);
            }
            catch (FormatException)
            {
                return TimeZoneInfoModel.CreateInvalid(AppConstants.Messages.INVALID_ZONE_FORMAT);
            }
            catch (OverflowException)
            {
                return TimeZoneInfoModel.CreateInvalid(AppConstants.Messages.INVALID_ZONE_FORMAT);
            }
        }

        public DateTime ConvertToLocalTime(DateTime utcTime, TimeZoneInfoModel timeZone)
        {
            if (!timeZone.IsValid)
                throw new ArgumentException("TimeZone invalid");

            return utcTime.AddHours(timeZone.OffsetHours);
        }

        public bool IsValidTimeZoneOffset(int offsetHours)
        {
            return offsetHours >= AppConstants.Validation.MIN_TIMEZONE_OFFSET &&
                   offsetHours <= AppConstants.Validation.MAX_TIMEZONE_OFFSET;
        }

        public string FormatTimeZoneString(int offsetHours)
        {
            if (offsetHours == 0)
                return AppConstants.TimeZone.GMT_PREFIX;

            if (offsetHours > 0)
                return $"{AppConstants.TimeZone.GMT_PLUS_PREFIX}{offsetHours}";

            return $"GMT{offsetHours}"; 
        }
    }
}
