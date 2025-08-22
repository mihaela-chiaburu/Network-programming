using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTPClientApp.Interfaces
{
    public interface INtpPacketService
    {
        byte[] CreateNtpPacket();
        System.DateTime ParseNtpTimestamp(byte[] ntpData);
        bool ValidateNtpResponse(byte[] ntpData);
    }
}
