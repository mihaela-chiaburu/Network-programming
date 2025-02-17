using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private string username;

        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 65432);
                stream = client.GetStream();

                username = "User" + new Random().Next(1000, 9999);
                byte[] nameData = Encoding.ASCII.GetBytes(username);
                stream.Write(nameData, 0, nameData.Length);

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
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Dispatcher.Invoke(() => DisplayMessage(message, "Left"));
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (stream == null)
            {
                MessageBox.Show("Not connected to the server.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                string fullMessage = username + ": " + message; 
                byte[] data = Encoding.ASCII.GetBytes(fullMessage);
                try
                {
                    stream.Write(data, 0, data.Length);
                    DisplayMessage("Me: " + message, "Right");
                    MessageInput.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending message: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
