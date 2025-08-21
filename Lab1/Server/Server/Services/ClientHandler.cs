using Server.Constants;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ClientHandler : IClientHandler
    {
        public event Action<string, TcpClient> MessageReceived;
        public event Action<string> ClientDisconnected;
        public event Action<string> UsernameReceived;

        public string ClientUsername { get; private set; }
        public TcpClient Client { get; private set; }
        public bool IsConnected { get; private set; }

        public async Task HandleClientAsync(TcpClient client)
        {
            Client = client;
            IsConnected = true;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[AppConstants.Network.BUFFER_SIZE];

            try
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                ClientUsername = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                UsernameReceived?.Invoke(ClientUsername);

                while (IsConnected && client.Connected &&
                       (bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    MessageReceived?.Invoke(message, client);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(AppConstants.Messages.CLIENT_HANDLE_ERROR + ex.Message);
            }
            catch (ObjectDisposedException)
            {
                // client was disposed
            }
            finally
            {
                IsConnected = false;
                ClientDisconnected?.Invoke(ClientUsername ?? "Unknown");

                try
                {
                    client?.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error closing client connection: " + ex.Message);
                }
            }
        }

        public void Disconnect()
        {
            IsConnected = false;
            try
            {
                Client?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disconnecting client: " + ex.Message);
            }
        }
    }
}
