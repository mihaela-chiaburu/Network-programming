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
    public class MessageDisplayService : IMessageDisplay
    {
        public void DisplayMessage(string message, bool isFromCurrentUser, StackPanel container, ScrollViewer scrollViewer)
        {
            var msgBlock = new TextBlock
            {
                Text = message,
                FontSize = AppConstants.UI.MESSAGE_FONT_SIZE,
                Padding = new Thickness(10),
                Margin = new Thickness(
                    isFromCurrentUser ? 50 : 0,
                    5,
                    isFromCurrentUser ? 0 : 50,
                    5),
                Background = isFromCurrentUser ? Brushes.LightBlue : Brushes.LightGray,
                HorizontalAlignment = isFromCurrentUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = AppConstants.UI.MAX_MESSAGE_WIDTH
            };

            container.Children.Add(msgBlock);
            scrollViewer.ScrollToBottom();
        }

        public void ClearMessages(StackPanel container)
        {
            container.Children.Clear();
        }
    }
}
