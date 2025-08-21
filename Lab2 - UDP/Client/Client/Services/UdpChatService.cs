using Client.Constants;
using Client.Interfaces;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class UdpChatService : IUdpChatService, IDisposable
    {
        private UdpClient _udpClient;
        private readonly INetworkUtils _networkUtils;
        private readonly IMessageParser _messageParser;
        private ClientInfo _clientInfo;
        private bool _isInitialized;
        private bool _isReceiving;

        public event Action<string, MessageType> MessageReceived;
        public event Action<string> ConnectionStatusChanged;

        public string ClientIP => _clientInfo?.IP ?? string.Empty;
        public string ClientUsername => _clientInfo?.Username ?? string.Empty;
        public bool IsInitialized => _isInitialized;

        public UdpChatService(INetworkUtils networkUtils, IMessageParser messageParser)
        {
            _networkUtils = networkUtils ?? throw new ArgumentNullException(nameof(networkUtils));
            _messageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
        }

        public async Task InitializeAsync()
        {
            try
            {
                var ip = _networkUtils.GenerateRandomIP();
                var username = _networkUtils.GenerateRandomUsername();
                _clientInfo = new ClientInfo(ip, username);

                var localEndPoint = new IPEndPoint(IPAddress.Parse(ip), AppConstants.Network.UDP_PORT);
                _udpClient = new UdpClient(localEndPoint);
                _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

                _isInitialized = true;
                ConnectionStatusChanged?.Invoke(AppConstants.Messages.CLIENT_INITIALIZED);

                await StartReceivingAsync();
            }
            catch (Exception ex)
            {
                _isInitialized = false;
                ConnectionStatusChanged?.Invoke(AppConstants.Messages.INITIALIZATION_ERROR + ex.Message);
                throw;
            }
        }

        private async Task StartReceivingAsync()
        {
            _isReceiving = true;

            while (_isReceiving && _udpClient != null)
            {
                try
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();
                    string rawMessage = Encoding.UTF8.GetString(result.Buffer);

                    var parsedMessage = _messageParser.ParseMessage(rawMessage, _clientInfo.GetDisplayName());
                    MessageReceived?.Invoke(parsedMessage.Content, parsedMessage.Type);
                }
                catch (ObjectDisposedException)
                {
                    _isReceiving = false;
                }
                catch (Exception ex)
                {
                    if (_isReceiving) 
                    {
                        ConnectionStatusChanged?.Invoke(AppConstants.Messages.RECEIVE_ERROR + ex.Message);
                    }
                }
            }
        }

        public async Task SendBroadcastMessageAsync(string message)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("UDP client not initialized");

            try
            {
                string fullMessage = _messageParser.FormatBroadcastMessage(_clientInfo.GetDisplayName(), message);
                await SendMessageAsync(AppConstants.Network.BROADCAST_IP, fullMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(AppConstants.Messages.SEND_ERROR + ex.Message);
            }
        }

        public async Task SendPrivateMessageAsync(string targetIp, string message)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("UDP client not initialized");

            if (!_networkUtils.IsValidIPAddress(targetIp))
                throw new ArgumentException(AppConstants.Messages.INVALID_IP_ERROR);

            try
            {
                string privateMessage = _messageParser.FormatPrivateMessage(_clientInfo.GetDisplayName(), message);
                await SendMessageAsync(targetIp, privateMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(AppConstants.Messages.SEND_ERROR + ex.Message);
            }
        }

        private async Task SendMessageAsync(string targetIp, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(targetIp), AppConstants.Network.UDP_PORT);

            await _udpClient.SendAsync(buffer, buffer.Length, remoteEndPoint);
        }

        public void Dispose()
        {
            _isReceiving = false;
            _isInitialized = false;

            _udpClient?.Close();
            _udpClient?.Dispose();
            _udpClient = null;

            ConnectionStatusChanged?.Invoke(AppConstants.Messages.CLIENT_DISPOSED);
        }
    }
}
