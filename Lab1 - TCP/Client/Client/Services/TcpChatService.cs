using Client.Constants;
using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class TcpChatService : IChat, IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;
        private bool _isConnected;

        public event Action<string> MessageReceived;
        public event Action<string> ConnectionStatusChanged;

        public string Username { get; private set; }
        public bool IsConnected => _isConnected && _client?.Connected == true;

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(AppConstants.Network.SERVER_IP, AppConstants.Network.SERVER_PORT);
                _stream = _client.GetStream();
                _isConnected = true;

                // Generate username
                Username = AppConstants.UI.USER_PREFIX + new Random().Next(1000, 9999);

                // Send username to server
                byte[] nameData = Encoding.ASCII.GetBytes(Username);
                await _stream.WriteAsync(nameData, 0, nameData.Length);

                _receiveThread = new Thread(ReceiveMessages)
                {
                    IsBackground = true
                };
                _receiveThread.Start();

                ConnectionStatusChanged?.Invoke("Connected to server");
                return true;
            }
            catch (Exception ex)
            {
                _isConnected = false;
                ConnectionStatusChanged?.Invoke(AppConstants.Messages.CONNECTION_ERROR + ex.Message);
                return false;
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (!IsConnected)
                throw new InvalidOperationException(AppConstants.Messages.NOT_CONNECTED);

            string fullMessage = Username + ": " + message;
            byte[] data = Encoding.ASCII.GetBytes(fullMessage);

            try
            {
                await _stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                throw new IOException(AppConstants.Messages.SEND_ERROR + ex.Message);
            }
        }

        private void ReceiveMessages()
        {
            byte[] buffer = new byte[AppConstants.Network.BUFFER_SIZE];
            int bytesRead;

            try
            {
                while (_isConnected && (bytesRead = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    MessageReceived?.Invoke(message);
                }
            }
            catch (IOException)
            {
                MessageReceived?.Invoke(AppConstants.Messages.SERVER_DISCONNECTED);
            }
            finally
            {
                _isConnected = false;
            }
        }

        public void Disconnect()
        {
            _isConnected = false;
            _stream?.Close();
            _client?.Close();
            _receiveThread?.Join(1000); 
        }

        public void Dispose()
        {
            Disconnect();
            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}
