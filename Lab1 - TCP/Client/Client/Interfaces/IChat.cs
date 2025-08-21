using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IChat
    {
        event Action<string> MessageReceived;
        event Action<string> ConnectionStatusChanged;

        Task<bool> ConnectAsync();
        Task SendMessageAsync(string message);
        void Disconnect();
        bool IsConnected { get; }
        string Username { get; }
    }
}
