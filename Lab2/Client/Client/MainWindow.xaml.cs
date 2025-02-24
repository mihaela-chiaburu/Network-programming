using System;
using System.Collections.Generic;
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
        private IPEndPoint serverEndPoint;
        private string username;
        private IPEndPoint selectedUserEndPoint = null;
        private Dictionary<string, IPEndPoint> userList = new Dictionary<string, IPEndPoint>();

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                udpClient = new UdpClient();
                serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65432);
                
                username = "User" + new Random().Next(1000, 9999);
                this.Title = $"{username} Chat";
                
                byte[] nameData = Encoding.ASCII.GetBytes(username);
                udpClient.Send(nameData, nameData.Length, serverEndPoint);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReceiveMessages()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    string message = Encoding.ASCII.GetString(data);

                    if (message.StartsWith("[USERLIST]"))
                    {
                        string[] users = message.Substring(10).Split(',');
                        Dispatcher.Invoke(() => UpdateUserList(users));
                    }
                    else if (message.StartsWith("[PRIVATE]"))
                    {
                        string[] parts = message.Split(new[] { '|' }, 2);
                        string sender = parts[0].Substring(9); // Remove "[PRIVATE]"
                        string privateMessage = parts[1];
                        Dispatcher.Invoke(() => DisplayMessage($"[Private from {sender}]: {privateMessage}", "Left"));
                    }
                    else if (message.StartsWith("[ERROR]"))
                    {
                        Dispatcher.Invoke(() => DisplayMessage(message, "Left"));
                    }
                    else
                    {
                        // general message
                        Dispatcher.Invoke(() => DisplayMessage(message, "Left"));
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => DisplayMessage("Error receiving message: " + ex.Message, "Left"));
                }
            }
        }

        private void UpdateUserList(string[] users)
        {
            ChannelsList.Items.Clear();
            ChannelsList.Items.Add(new ListBoxItem { Content = "General", Tag = "General" });

            userList.Clear();

            foreach (var user in users)
            {
                string[] parts = user.Split('|');
                string username = parts[0];
                string[] ipParts = parts[1].Split(':');
                IPAddress ip = IPAddress.Parse(ipParts[0]);
                int port = int.Parse(ipParts[1]);

                userList[username] = new IPEndPoint(ip, port);

                ChannelsList.Items.Add(new ListBoxItem { Content = username, Tag = username });
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                if (selectedUserEndPoint != null)
                {
                    if (ChannelsList.SelectedItem is ListBoxItem selectedItem)
                    {
                        string recipientUsername = selectedItem.Content.ToString(); // extract the username
                        string privateMessage = $"[PRIVATE]{recipientUsername}|{message}";
                        byte[] data = Encoding.ASCII.GetBytes(privateMessage);
                        udpClient.Send(data, data.Length, serverEndPoint);
                    }
                    else
                    {
                        MessageBox.Show("Please select a user to send a private message.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    //general message
                    string fullMessage = username + ": " + message;
                    byte[] data = Encoding.ASCII.GetBytes(fullMessage);
                    udpClient.Send(data, data.Length, serverEndPoint);
                }
                DisplayMessage("Me: " + message, "Right");
                MessageInput.Clear();
            }
        }

        private void DisplayMessage(string message, string side)
        {
            TextBlock msgBlock = new TextBlock
            {
                Text = message,
                FontSize = 18,
                Padding = new Thickness(10),
                Margin = new Thickness(side == "Right" ? 50 : 0, 5, side == "Left" ? 50 : 0, 5),
                Background = (side == "Right") ? System.Windows.Media.Brushes.LightBlue : System.Windows.Media.Brushes.LightGray,
                HorizontalAlignment = (side == "Right") ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400
            };

            ChatMessagesPanel.Children.Add(msgBlock);
            ChatScrollViewer.ScrollToBottom();
        }

        private void ChannelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ChannelsList.SelectedItem is ListBoxItem selectedItem)
                {
                    string selectedUser = selectedItem.Tag.ToString();
                    if (selectedUser != "General")
                    {
                        if (userList.ContainsKey(selectedUser))
                        {
                            selectedUserEndPoint = userList[selectedUser];
                        }
                        else
                        {
                            MessageBox.Show($"User '{selectedUser}' not found in the user list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        selectedUserEndPoint = null; // General chat
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == "Type your message here...")
            {
                MessageInput.Text = "";
                MessageInput.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                MessageInput.Text = "Type your message here...";
                MessageInput.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}