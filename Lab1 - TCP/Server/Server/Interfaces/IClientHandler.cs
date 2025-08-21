using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IClientHandler
    {
        event Action<string, TcpClient> MessageReceived;
        event Action<string> ClientDisconnected;
        event Action<string> UsernameReceived;

        Task HandleClientAsync(TcpClient client);
        string ClientUsername { get; }
        TcpClient Client { get; }
        bool IsConnected { get; }
    }
}
