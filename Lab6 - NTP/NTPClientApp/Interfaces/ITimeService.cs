using NTPClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Interfaces
{
    public interface ITimeService
    {
        Task<TimeResult> GetTimeForZoneAsync(string zoneString, string ntpServer = null);
        Task<TimeResult> GetUtcTimeAsync(string ntpServer = null);
    }
}
