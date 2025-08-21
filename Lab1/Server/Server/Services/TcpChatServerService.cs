using Server.Constants;
using Server.Interfaces;
using Server.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class TcpChatServerService : IChatServerService, IDisposable
    {
        private TcpListener _server;
        private readonly ConcurrentDictionary<TcpClient, ClientInfo> _clients;
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action<string> MessageReceived;
        public event Action<string> ClientJoined;
        public event Action<string> ClientDisconnected;
        public event Action<string> ServerStatusChanged;

        public bool IsRunning => _isRunning;
        public int ConnectedClientsCount => _clients.Count(kvp => kvp.Value.IsConnected);
        public IReadOnlyList<string> ConnectedClients =>
            _clients.Values.Where(c => c.IsConnected).Select(c => c.Username).ToList().AsReadOnly();

        public TcpChatServerService()
        {
            _clients = new ConcurrentDictionary<TcpClient, ClientInfo>();
        }

        public async Task StartAsync()
        {
            try
            {
                _server = new TcpListener(IPAddress.Any, AppConstants.Network.SERVER_PORT);
                _server.Start();
                _isRunning = true;
                _cancellationTokenSource = new CancellationTokenSource();

                ServerStatusChanged?.Invoke(AppConstants.Messages.SERVER_STARTED + AppConstants.Network.SERVER_PORT);

                _ = Task.Run(async () => await AcceptClientsAsync(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                _isRunning = false;
                ServerStatusChanged?.Invoke(AppConstants.Messages.SERVER_START_ERROR + ex.Message);
                throw;
            }
        }

        public async Task StopAsync()
        {
            _isRunning = false;
            _cancellationTokenSource?.Cancel();

            foreach (var kvp in _clients.ToList())
            {
                try
                {
                    kvp.Key.Close();
                    _clients.TryRemove(kvp.Key, out _);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error closing client connection: " + ex.Message);
                }
            }

            try
            {
                _server?.Stop();
                ServerStatusChanged?.Invoke(AppConstants.Messages.SERVER_STOPPED);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error stopping server: " + ex.Message);
            }
        }

        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var tcpClient = await _server.AcceptTcpClientAsync();
                    var clientInfo = new ClientInfo(tcpClient, "Unknown");
                    _clients.TryAdd(tcpClient, clientInfo);

                    _ = Task.Run(async () => await HandleClientAsync(tcpClient, clientInfo));
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (_isRunning) 
                    {
                        Console.WriteLine("Error accepting client: " + ex.Message);
                    }
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, ClientInfo clientInfo)
        {
            var clientHandler = new ClientHandler();
            bool joinMessageSent = false;

            clientHandler.MessageReceived += (message, sender) =>
            {
                MessageReceived?.Invoke(message);
                BroadcastMessage(message, sender);
            };

            clientHandler.ClientDisconnected += (username) =>
            {
                _clients.TryRemove(client, out _);
                if (!string.IsNullOrEmpty(username) && username != "Unknown")
                {
                    ClientDisconnected?.Invoke(username + AppConstants.Messages.CLIENT_LEFT);
                }
                else
                {
                    ClientDisconnected?.Invoke(AppConstants.Messages.CLIENT_DISCONNECTED);
                }
            };

            clientHandler.UsernameReceived += (username) =>
            {
                if (!joinMessageSent)
                {
                    clientInfo.Username = username;
                    ClientJoined?.Invoke(username + AppConstants.Messages.CLIENT_JOINED);
                    joinMessageSent = true;
                }
            };

            await clientHandler.HandleClientAsync(client);
        }

        public void BroadcastMessage(string message)
        {
            BroadcastMessage(message, null);
        }

        public void BroadcastFromServer(string message)
        {
            string serverMessage = AppConstants.UI.SERVER_PREFIX + message;
            BroadcastMessage(serverMessage, null);
        }

        private void BroadcastMessage(string message, TcpClient excludeClient)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            var clientsToRemove = new List<TcpClient>();

            foreach (var kvp in _clients.ToList())
            {
                if (kvp.Key != excludeClient && kvp.Value.IsConnected)
                {
                    try
                    {
                        var stream = kvp.Key.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(AppConstants.Messages.BROADCAST_ERROR + ex.Message);
                        clientsToRemove.Add(kvp.Key);
                    }
                }
            }

            foreach (var clientToRemove in clientsToRemove)
            {
                _clients.TryRemove(clientToRemove, out _);
                try
                {
                    clientToRemove.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error closing disconnected client: " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            Task.Run(async () => await StopAsync()).Wait();
            _cancellationTokenSource?.Dispose();
            _server = null;
        }
    }
}
