using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ClientInfo
    {
        public string IP { get; set; }
        public string Username { get; set; }
        public DateTime ConnectedAt { get; set; }

        public ClientInfo(string ip, string username)
        {
            IP = ip;
            Username = username;
            ConnectedAt = DateTime.Now;
        }

        public string GetDisplayName()
        {
            return $"{IP} ({Username})";
        }
    }
}
