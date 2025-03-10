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
            clientIP = GetRandomIpAddress(); // Generate a random IP address
            clientUsername = $"User{new Random().Next(1000)}";

            this.Title = $"Chat Client - {clientIP} ({clientUsername})";

            // Create the UDP client and bind it to the unique IP
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(clientIP), Port));

            // Allow the socket to receive broadcast messages
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            // Start listening for incoming messages
            StartReceiving();
        }

        private string GetRandomIpAddress()
        {
            // Generate a random IP address in the range 127.0.0.2 to 127.0.0.254
            Random random = new Random();
            int lastOctet = random.Next(2, 255); // Exclude 127.0.0.1 and 127.0.0.255
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

                    // Check if the message is a private message
                    if (message.StartsWith("PRIVATE_FROM:"))
                    {
                        // Extract the sender's IP, username, and message
                        var parts = message.Substring("PRIVATE_FROM:".Length).Split(new[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            string senderInfo = parts[0].Trim();
                            string privateMessage = parts[1].Trim();

                            // Check if the message is from the current user
                            if (senderInfo == $"{clientIP} ({clientUsername})")
                            {
                                // Display the message as "Me"
                                DisplayMessage($"Me: {privateMessage}", "Right");
                            }
                            else
                            {
                                // Display the private message with the format "private message from ... : message"
                                DisplayMessage($"private message from {senderInfo}: {privateMessage}", "Left");
                            }
                        }
                    }
                    else
                    {
                        // Check if the message is from the current user
                        if (message.StartsWith($"{clientIP} ({clientUsername}):"))
                        {
                            // Display the message as "Me"
                            DisplayMessage($"Me: {message.Substring($"{clientIP} ({clientUsername}):".Length).Trim()}", "Right");
                        }
                        else
                        {
                            // Display general messages normally
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
                // Create a TextBlock for the message
                TextBlock msgBlock = new TextBlock
                {
                    Text = message,
                    FontSize = 18,
                    Padding = new Thickness(10),
                    Margin = new Thickness(side == "Right" ? 50 : 20, 5, side == "Left" ? 0 : 20, 5), // Adjust margins
                    Background = (side == "Right") ? System.Windows.Media.Brushes.LightBlue : System.Windows.Media.Brushes.LightGray,
                    HorizontalAlignment = (side == "Right") ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 400
                };

                // Add the message to the chat window
                ChatMessagesPanel.Children.Add(msgBlock);

                // Scroll to the bottom of the chat window
                ChatScrollViewer.ScrollToBottom();
            });
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (string.IsNullOrWhiteSpace(message)) return;

            // Prepend the client's unique virtual IP and username to the message
            string fullMessage = $"{clientIP} ({clientUsername}): {message}";

            // Check if the message is private
            if (message.StartsWith("private "))
            {
                // Extract user IP and message
                var parts = message.Substring(8).Split(':');
                if (parts.Length == 2)
                {
                    string userIp = parts[0].Trim();
                    string userMessage = parts[1].Trim();

                    // Send the private message with the prefix "PRIVATE_FROM:"
                    SendMessage(userIp, $"PRIVATE_FROM:{clientIP} ({clientUsername}): {userMessage}");
                }
            }
            else
            {
                // General message to broadcast address
                SendMessage("255.255.255.255", fullMessage);  // Broadcast address
            }

            // Clear the input box
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