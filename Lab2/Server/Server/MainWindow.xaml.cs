using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Server
{
    public partial class MainWindow : Window
    {
        private UdpClient udpServer;
        private Dictionary<IPEndPoint, string> clients = new Dictionary<IPEndPoint, string>();
        private bool isRunning = true;
        private const int Port = 65432;
        private IPEndPoint selectedUserEndPoint = null;

        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            udpServer = new UdpClient(Port);
            Thread serverThread = new Thread(ReceiveMessages);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void ReceiveMessages()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);
            while (isRunning)
            {
                try
                {
                    byte[] data = udpServer.Receive(ref remoteEndPoint);
                    string message = Encoding.ASCII.GetString(data);

                    if (!clients.ContainsKey(remoteEndPoint))
                    {
                        clients[remoteEndPoint] = message;
                        Dispatcher.Invoke(() => AddUserToChannelsList(message));
                        Dispatcher.Invoke(() => DisplayMessage(message + " joined the chat.", "Left"));
                        BroadcastUserList();
                    }
                    else if (message.StartsWith("[PRIVATE]"))
                    {
                        string[] parts = message.Split(new[] { '|' }, 2);
                        string recipientUsername = parts[0].Substring(9); // remove "[PRIVATE]"
                        string privateMessage = parts[1];

                        var recipient = clients.FirstOrDefault(x => x.Value == recipientUsername).Key;
                        if (recipient != null)
                        {
                            //private message 
                            string senderUsername = clients[remoteEndPoint];
                            byte[] privateData = Encoding.ASCII.GetBytes($"[PRIVATE]{senderUsername}|{privateMessage}");
                            udpServer.Send(privateData, privateData.Length, recipient);
                        }
                        else
                        {
                            byte[] errorData = Encoding.ASCII.GetBytes($"[ERROR]User '{recipientUsername}' not found.");
                            udpServer.Send(errorData, errorData.Length, remoteEndPoint);
                        }
                    }
                    else
                    {
                        // general message
                        Dispatcher.Invoke(() => DisplayMessage(message, "Left"));
                        BroadcastMessage(message, remoteEndPoint);
                    }
                }
                catch (IOException ex1)
                {
                    //client disconnection
                    if (clients.ContainsKey(remoteEndPoint))
                    {
                        string username = clients[remoteEndPoint];
                        clients.Remove(remoteEndPoint);
                        Dispatcher.Invoke(() => DisplayMessage($"{username} has disconnected.", "Left"));
                        BroadcastUserList();
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => DisplayMessage("Error receiving message: " + ex.Message, "Left"));
                }
            }
        }

        private void AddUserToChannelsList(string username)
        {
            ListBoxItem userItem = new ListBoxItem
            {
                Content = username,
                Tag = username
            };
            ChannelsList.Items.Add(userItem);
        }

        private void BroadcastMessage(string message, IPEndPoint senderEndPoint)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            foreach (var client in clients)
            {
                if (!client.Key.Equals(senderEndPoint))
                {
                    udpServer.Send(data, data.Length, client.Key);
                }
            }
        }

        private void BroadcastUserList()
        {
            var userList = clients.Select(c => $"{c.Value}|{c.Key.Address}:{c.Key.Port}").ToList();
            string userListMessage = "[USERLIST]" + string.Join(",", userList);
            byte[] data = Encoding.ASCII.GetBytes(userListMessage);

            //the user list to all clients
            foreach (var client in clients.Keys)
            {
                udpServer.Send(data, data.Length, client);
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

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                DisplayMessage(message, "Right");
                BroadcastMessage(message, null);
                MessageInput.Clear();
            }
        }

        private void ChannelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelsList.SelectedItem is ListBoxItem selectedItem)
            {
                string selectedUser = selectedItem.Tag.ToString();
                if (selectedUser != "General")
                {
                    selectedUserEndPoint = clients.FirstOrDefault(x => x.Value == selectedUser).Key;
                }
                else
                {
                    selectedUserEndPoint = null; // General chat
                }
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