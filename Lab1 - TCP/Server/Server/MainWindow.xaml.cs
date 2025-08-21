using Server.Constants;
using Server.Interfaces;
using Server.Services;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Server
{
    public partial class MainWindow : Window
    {
        private readonly IChatServerService _serverService;
        private readonly IMessageDisplayService _messageDisplayService;

        public MainWindow() : this(new TcpChatServerService(), new MessageDisplayService())
        {
        }

        public MainWindow(IChatServerService serverService, IMessageDisplayService messageDisplayService)
        {
            InitializeComponent();
            _serverService = serverService;
            _messageDisplayService = messageDisplayService;

            InitializeServices();
            StartServer();
        }

        private void InitializeServices()
        {
            _serverService.MessageReceived += OnMessageReceived;
            _serverService.ClientJoined += OnClientJoined;
            _serverService.ClientDisconnected += OnClientDisconnected;
            _serverService.ServerStatusChanged += OnServerStatusChanged;
        }

        private async void StartServer()
        {
            try
            {
                await _serverService.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    AppConstants.Messages.SERVER_START_ERROR + ex.Message,
                    AppConstants.Titles.SERVER_ERROR,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnMessageReceived(string message)
        {
            Dispatcher.Invoke(() => _messageDisplayService.DisplayMessage(message, false, ChatMessagesPanel, ChatScrollViewer));
        }

        private void OnClientJoined(string message)
        {
            Dispatcher.Invoke(() => _messageDisplayService.DisplaySystemMessage(message, ChatMessagesPanel, ChatScrollViewer));
        }

        private void OnClientDisconnected(string message)
        {
            Dispatcher.Invoke(() => _messageDisplayService.DisplaySystemMessage(message, ChatMessagesPanel, ChatScrollViewer));
        }

        private void OnServerStatusChanged(string status)
        {
            Dispatcher.Invoke(() =>
            {
                _messageDisplayService.DisplaySystemMessage($"Server Status: {status}", ChatMessagesPanel, ChatScrollViewer);
                Console.WriteLine($"Server Status: {status}");
            });
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!_serverService.IsRunning)
            {
                MessageBox.Show(
                    "Server is not running.",
                    AppConstants.Titles.SERVER_ERROR,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string message = MessageInput.Text.Trim();
            if (!string.IsNullOrEmpty(message) && message != AppConstants.UI.PLACEHOLDER_TEXT)
            {
                try
                {
                    _messageDisplayService.DisplayMessage(
                        AppConstants.UI.SERVER_PREFIX + message,
                        true,
                        ChatMessagesPanel,
                        ChatScrollViewer);

                    _serverService.BroadcastFromServer(message);
                    MessageInput.Clear();
                    MessageInput.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error sending message: " + ex.Message,
                        AppConstants.Titles.SERVER_ERROR,
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
            _serverService?.StopAsync().Wait();
            if (_serverService is IDisposable disposable)
                disposable.Dispose();
            base.OnClosed(e);
        }
    }
}
