using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ClientInfo
    {
        public TcpClient TcpClient { get; set; }
        public string Username { get; set; }
        public DateTime ConnectedAt { get; set; }
        public bool IsConnected { get; set; }

        public ClientInfo(TcpClient client, string username)
        {
            TcpClient = client;
            Username = username;
            ConnectedAt = DateTime.Now;
            IsConnected = true;
        }
    }
}
