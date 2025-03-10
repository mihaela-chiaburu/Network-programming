using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public partial class MainWindow : Window
    {
        private UdpClient udpClient;
        private const int Port = 9000;  // Port for UDP
        private string myIp;  // This will hold the unique virtual IP for the client
        private string myUsername;  // Random username for the client

        public MainWindow()
        {
            InitializeComponent();

            // Get the unique IP for the client in the range 127.0.0.1 to 127.0.0.254
            myIp = GetUniqueIpAddress();  // Use a unique IP for each client (127.0.0.1, 127.0.0.2, etc.)

            // Assign a random username
            myUsername = $"User{new Random().Next(1000)}";

            // Set the window title to include the client's IP and username
            this.Title = $"Chat Client - {myIp} ({myUsername})";

            // Create the UDP client and bind it to the unique IP
            udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(myIp), Port));

            // Allow the socket to receive broadcast messages
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            // Start listening for incoming messages
            StartReceiving();
        }

        private string GetUniqueIpAddress()
        {
            string ipCounterFile = "ip_counter.txt";
            int clientCounter = 1;

            // Use a Mutex to ensure thread-safe access to the file
            using (Mutex mutex = new Mutex(false, "Global\\IPCounterMutex"))
            {
                mutex.WaitOne();  // Wait until it is safe to access the file

                try
                {
                    // Read the last assigned IP counter from the file
                    if (File.Exists(ipCounterFile))
                    {
                        string counterText = File.ReadAllText(ipCounterFile);
                        if (int.TryParse(counterText, out int lastCounter))
                        {
                            clientCounter = lastCounter + 1;
                        }
                    }

                    // Ensure the counter stays within the valid range (1 to 254)
                    if (clientCounter > 254)
                    {
                        clientCounter = 1;  // Reset to 1 if we exceed 254
                    }

                    // Write the new counter value back to the file
                    File.WriteAllText(ipCounterFile, clientCounter.ToString());
                }
                finally
                {
                    mutex.ReleaseMutex();  // Release the Mutex
                }
            }

            return $"127.0.0.{clientCounter}";
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

                            // Display the private message with the format "private message from ... : message"
                            DisplayMessage($"private message from {senderInfo}: {privateMessage}");
                        }
                    }
                    else
                    {
                        // Display general messages normally
                        DisplayMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error receiving message: {ex.Message}");
                }
            }
        }

        private void DisplayMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                // Display the message in the chat window
                var messageTextBlock = new TextBlock
                {
                    Text = message,  // Show the message
                    Margin = new Thickness(5)
                };
                ChatMessagesPanel.Children.Add(messageTextBlock);
            });
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            if (string.IsNullOrWhiteSpace(message)) return;

            // Prepend the client's unique virtual IP and username to the message
            string fullMessage = $"{myIp} ({myUsername}): {message}";

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
                    SendMessage(userIp, $"PRIVATE_FROM:{myIp} ({myUsername}): {userMessage}");
                }
            }
            else
            {
                // General message to broadcast address
                SendMessage("255.255.255.255", fullMessage);  // Broadcast address
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

        protected override void OnClosed(EventArgs e)
        {
            // Reset the IP counter file when the application is closed
            File.WriteAllText("ip_counter.txt", "1");
            base.OnClosed(e);
        }
    }
}