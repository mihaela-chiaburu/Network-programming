using Client.Constants;
using Client.Interfaces;
using Client.Services;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client
{
    public partial class MainWindow : Window
    {
        private readonly IChat _chat;
        private readonly IMessageDisplay _messageDisplay;

        public MainWindow() : this(new TcpChatService(), new MessageDisplayService())
        {
        }

        public MainWindow(IChat chatService, IMessageDisplay messageDisplayService)
        {
            InitializeComponent();
            _chat = chatService;
            _messageDisplay = messageDisplayService;

            InitializeServices();
            ConnectToServer();
        }

        private void InitializeServices()
        {
            _chat.MessageReceived += OnMessageReceived;
            _chat.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        private async void ConnectToServer()
        {
            var connected = await _chat.ConnectAsync();
            if (!connected)
            {
                MessageBox.Show(
                    AppConstants.Messages.CONNECTION_ERROR,
                    AppConstants.Messages.CONNECTION_ERROR_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnMessageReceived(string message)
        {
            Dispatcher.Invoke(() => _messageDisplay.DisplayMessage(message, false, ChatMessagesPanel, ChatScrollViewer));
        }

        private void OnConnectionStatusChanged(string status)
        {
            Dispatcher.Invoke(() => {
                Console.WriteLine($"Connection Status: {status}");
            });
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!_chat.IsConnected)
            {
                MessageBox.Show(
                    AppConstants.Messages.NOT_CONNECTED,
                    AppConstants.Messages.ERROR_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(message) && message != AppConstants.UI.PLACEHOLDER_TEXT)
            {
                try
                {
                    await _chat.SendMessageAsync(message);
                    _messageDisplay.DisplayMessage(
                        AppConstants.UI.ME_PREFIX + message,
                        true,
                        ChatMessagesPanel,
                        ChatScrollViewer);
                    MessageInput.Clear();
                    MessageInput.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        AppConstants.Messages.ERROR_TITLE,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == AppConstants.UI.PLACEHOLDER_TEXT)
            {
                MessageInput.Text = "";
                MessageInput.Foreground = Brushes.Black;
            }
        }

        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                MessageInput.Text = AppConstants.UI.PLACEHOLDER_TEXT;
                MessageInput.Foreground = Brushes.Gray;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _chat?.Disconnect();
            if (_chat is IDisposable disposable)
                disposable.Dispose();
            base.OnClosed(e);
        }
    }
}
