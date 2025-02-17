using System;
using System.Collections.Generic;
using System.IO;
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
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private bool isRunning = true;

        public MainWindow()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            server = new TcpListener(IPAddress.Any, 65432);
            server.Start();
            Thread serverThread = new Thread(AcceptClients);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void AcceptClients()
        {
            while (isRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.IsBackground = true;
                clientThread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string username = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Dispatcher.Invoke(() => DisplayMessage(username + " joined the chat.", "Left"));

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Dispatcher.Invoke(() => DisplayMessage(message, "Left"));
                    BroadcastMessage(message, client);
                }
            }
            catch (IOException ex)
            {
                Dispatcher.Invoke(() => DisplayMessage("A client has disconnected unexpectedly.", "Left"));
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                clients.Remove(client);
                client.Close();
            }
        }



        private void BroadcastMessage(string message, TcpClient sender)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            foreach (var client in clients)
            {
                if (client != sender)
                {
                    client.GetStream().Write(data, 0, data.Length);
                }
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
