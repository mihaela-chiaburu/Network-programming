using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTPClientApp.Models;

namespace NTPClientApp.Interfaces
{
    public interface ITimeZoneService
    {
        TimeZoneInfoModel ParseTimeZone(string zoneString);
        DateTime ConvertToLocalTime(DateTime utcTime, TimeZoneInfoModel timeZone);
        bool IsValidTimeZoneOffset(int offsetHours);
        string FormatTimeZoneString(int offsetHours);
    }
}
