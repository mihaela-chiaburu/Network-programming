using Client.Constants;
using Client.Interfaces;
using Client.Services;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client
{
    public partial class MainWindow : Window
    {
        private readonly IUdpChatService _udpChatService;
        private readonly IMessageDisplayService _messageDisplayService;

        public MainWindow() : this(
            new UdpChatService(new NetworkUtils(), new MessageParser()),
            new MessageDisplayService())
        {
        }

        public MainWindow(IUdpChatService udpChatService, IMessageDisplayService messageDisplayService)
        {
            InitializeComponent();

            _udpChatService = udpChatService ?? throw new ArgumentNullException(nameof(udpChatService));
            _messageDisplayService = messageDisplayService ?? throw new ArgumentNullException(nameof(messageDisplayService));

            InitializeServices();
            InitializeUdpClient();
        }

        private void InitializeServices()
        {
            _udpChatService.MessageReceived += OnMessageReceived;
            _udpChatService.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        private async void InitializeUdpClient()
        {
            try
            {
                await _udpChatService.InitializeAsync();

                this.Title = string.Format(
                    AppConstants.Format.WINDOW_TITLE,
                    _udpChatService.ClientIP,
                    _udpChatService.ClientUsername);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    AppConstants.Messages.INITIALIZATION_ERROR + ex.Message,
                    AppConstants.Titles.ERROR_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnMessageReceived(string message, MessageType messageType)
        {
            Dispatcher.Invoke(() =>
                _messageDisplayService.DisplayMessage(message, messageType, ChatMessagesPanel, ChatScrollViewer));
        }

        private void OnConnectionStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                Console.WriteLine($"UDP Client Status: {status}");
            });
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(message) || message == AppConstants.UI.PLACEHOLDER_TEXT)
                return;

            if (!_udpChatService.IsInitialized)
            {
                MessageBox.Show(
                    "UDP client is not initialized.",
                    AppConstants.Titles.ERROR_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (message.StartsWith(AppConstants.Network.PRIVATE_COMMAND_PREFIX))
                {
                    await HandlePrivateMessage(message);
                }
                else
                {
                    await _udpChatService.SendBroadcastMessageAsync(message);
                }

                MessageInput.Clear();
                MessageInput.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    AppConstants.Titles.ERROR_TITLE,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task HandlePrivateMessage(string message)
        {
            var privateContent = message.Substring(AppConstants.Network.PRIVATE_COMMAND_PREFIX.Length);
            var parts = privateContent.Split(new[] { AppConstants.Format.PRIVATE_MESSAGE_SEPARATOR }, 2);

            if (parts.Length != 2)
            {
                throw new ArgumentException(AppConstants.Messages.PRIVATE_MESSAGE_FORMAT_ERROR);
            }

            string targetIp = parts[0].Trim();
            string messageContent = parts[1].Trim();

            if (string.IsNullOrWhiteSpace(targetIp) || string.IsNullOrWhiteSpace(messageContent))
            {
                throw new ArgumentException(AppConstants.Messages.PRIVATE_MESSAGE_FORMAT_ERROR);
            }

            string localDisplay = $"{AppConstants.UI.PRIVATE_TO_PREFIX}{targetIp}): {messageContent}";
            _messageDisplayService.DisplayMessage(localDisplay, MessageType.PrivateSent, ChatMessagesPanel, ChatScrollViewer);

            await _udpChatService.SendPrivateMessageAsync(targetIp, messageContent);
        }

        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MessageInput.Text == AppConstants.UI.PLACEHOLDER_TEXT)
            {
                MessageInput.Text = string.Empty;
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
            _udpChatService?.Dispose();
            base.OnClosed(e);
        }
    }
}