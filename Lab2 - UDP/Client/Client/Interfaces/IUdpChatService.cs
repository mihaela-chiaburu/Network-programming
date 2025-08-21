using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IUdpChatService
    {
        event Action<string, MessageType> MessageReceived;
        event Action<string> ConnectionStatusChanged;

        Task InitializeAsync();
        Task SendBroadcastMessageAsync(string message);
        Task SendPrivateMessageAsync(string targetIp, string message);
        void Dispose();

        string ClientIP { get; }
        string ClientUsername { get; }
        bool IsInitialized { get; }
    }

    public enum MessageType
    {
        Broadcast,
        PrivateReceived,
        PrivateSent,
        System
    }
}
