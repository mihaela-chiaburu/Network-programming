using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public partial class MainWindow : Window
    {
        private UdpClient udpClient;
        private const int Port = 9000;
        private string clientIP;
        private string clientUsername;

        public MainWindow()
        {
            InitializeComponent();

            clientIP = GetRandomIpAddress();
            clientUsername = $"User{new Random().Next(1000)}";

            this.Title = $"Chat Client - {clientIP} ({clientUsername})";

            udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(clientIP), Port));
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            StartReceiving();
        }

        private string GetRandomIpAddress()
        {
            Random random = new Random();
            int lastOctet = random.Next(2, 255);
            return $"127.0.0.{lastOctet}";
        }

        private async void StartReceiving()
        {
            while (true)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);

                    if (message.StartsWith("PRIVATE_FROM:"))
                    {
                        var parts = message.Substring("PRIVATE_FROM:".Length).Split(new[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            string senderInfo = parts[0].Trim();
                            string privateMessage = parts[1].Trim();

                            if (senderInfo == $"{clientIP} ({clientUsername})")
                            {
                                DisplayMessage($"Me: {privateMessage}", "Right");
                            }
                            else
                            {
                                DisplayMessage($"private message from {senderInfo}: {privateMessage}", "Left");
                            }
                        }
                    }
                    else
                    {
                        if (message.StartsWith($"{clientIP} ({clientUsername}):"))
                        {
                            DisplayMessage($"Me: {message.Substring($"{clientIP} ({clientUsername}):".Length).Trim()}", "Right");
                        }
                        else
                        {
                            DisplayMessage(message, "Left");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error receiving message: {ex.Message}");
                }
            }
        }

        private void DisplayMessage(string message, string side)
        {
            Dispatcher.Invoke(() =>
            {
                TextBlock msgBlock = new TextBlock
                {
                    Text = message,
                    FontSize = 18,
                    Padding = new Thickness(10),
                    Margin = new Thickness(side == "Right" ? 50 : 20, 5, side == "Left" ? 0 : 20, 5),
                    Background = (side == "Right") ? System.Windows.Media.Brushes.LightBlue : System.Windows.Media.Brushes.LightGray,
                    HorizontalAlignment = (side == "Right") ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 400
                };

                ChatMessagesPanel.Children.Add(msgBlock);

                ChatScrollViewer.ScrollToBottom();
            });
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (string.IsNullOrWhiteSpace(message)) return;

            string fullMessage = $"{clientIP} ({clientUsername}): {message}";

            if (message.StartsWith("private "))
            {
                var parts = message.Substring(8).Split(':');
                if (parts.Length == 2)
                {
                    string userIp = parts[0].Trim();
                    string userMessage = parts[1].Trim();

                    DisplayMessage($"Me (to {userIp}): {userMessage}", "Right");
                    SendMessage(userIp, $"PRIVATE_FROM:{clientIP} ({clientUsername}): {userMessage}");
                }
            }
            else
            {
                SendMessage("255.255.255.255", fullMessage);
            }

            MessageInput.Clear();
        }

        private void SendMessage(string ip, string message)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), Port);
                udpClient.Send(buffer, buffer.Length, remoteEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == "Type your message here...")
                MessageInput.Text = string.Empty;
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageInput.Text))
                MessageInput.Text = "Type your message here...";
        }
    }
}