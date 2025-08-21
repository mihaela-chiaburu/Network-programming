using Server.Constants;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Server.Services
{
    public class MessageDisplayService : IMessageDisplayService
    {
        public void DisplayMessage(string message, bool isFromServer, StackPanel container, ScrollViewer scrollViewer)
        {
            var msgBlock = new TextBlock
            {
                Text = message,
                FontSize = AppConstants.UI.MESSAGE_FONT_SIZE,
                Padding = new Thickness(10),
                Margin = new Thickness(
                    isFromServer ? 50 : 0,
                    5,
                    isFromServer ? 0 : 50,
                    5),
                Background = isFromServer ? Brushes.LightBlue : Brushes.LightGray,
                HorizontalAlignment = isFromServer ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = AppConstants.UI.MAX_MESSAGE_WIDTH
            };

            container.Children.Add(msgBlock);
            scrollViewer.ScrollToBottom();
        }

        public void DisplaySystemMessage(string message, StackPanel container, ScrollViewer scrollViewer)
        {
            var msgBlock = new TextBlock
            {
                Text = message,
                FontSize = AppConstants.UI.MESSAGE_FONT_SIZE - 2,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 0, 5),
                Background = Brushes.LightYellow,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = AppConstants.UI.MAX_MESSAGE_WIDTH,
                FontStyle = FontStyles.Italic
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
