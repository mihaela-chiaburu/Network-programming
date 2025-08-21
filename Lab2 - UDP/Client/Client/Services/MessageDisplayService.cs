using Client.Constants;
using Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.Services
{
    public class MessageDisplayService : IMessageDisplayService
    {
        public void DisplayMessage(string message, MessageType messageType, StackPanel container, ScrollViewer scrollViewer)
        {
            var (backgroundColor, alignment, margin) = GetMessageStyle(messageType);

            var msgBlock = new TextBlock
            {
                Text = message,
                FontSize = AppConstants.UI.MESSAGE_FONT_SIZE,
                Padding = new Thickness(10),
                Margin = margin,
                Background = backgroundColor,
                HorizontalAlignment = alignment,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = AppConstants.UI.MAX_MESSAGE_WIDTH
            };

            container.Children.Add(msgBlock);
            scrollViewer.ScrollToBottom();
        }

        private (Brush backgroundColor, HorizontalAlignment alignment, Thickness margin) GetMessageStyle(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Broadcast when IsFromCurrentClient(messageType) =>
                    (Brushes.LightBlue, HorizontalAlignment.Right, new Thickness(50, 5, 20, 5)),

                MessageType.PrivateSent =>
                    (Brushes.LightGreen, HorizontalAlignment.Right, new Thickness(50, 5, 20, 5)),

                MessageType.PrivateReceived =>
                    (Brushes.LightPink, HorizontalAlignment.Left, new Thickness(20, 5, 50, 5)),

                MessageType.System =>
                    (Brushes.LightYellow, HorizontalAlignment.Center, new Thickness(20, 5, 20, 5)),

                _ => 
                    (Brushes.LightGray, HorizontalAlignment.Left, new Thickness(20, 5, 0, 5))
            };
        }

        private bool IsFromCurrentClient(MessageType messageType)
        {
            return messageType == MessageType.PrivateSent;
        }

        public void ClearMessages(StackPanel container)
        {
            container.Children.Clear();
        }
    }
}
