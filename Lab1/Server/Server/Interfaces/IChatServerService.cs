using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfaces
{
    public interface IChatServerService
    {
        event Action<string> MessageReceived;
        event Action<string> ClientJoined;
        event Action<string> ClientDisconnected;
        event Action<string> ServerStatusChanged;

        Task StartAsync();
        Task StopAsync();
        void BroadcastMessage(string message);
        void BroadcastFromServer(string message);
        bool IsRunning { get; }
        int ConnectedClientsCount { get; }
        IReadOnlyList<string> ConnectedClients { get; }
    }
}
