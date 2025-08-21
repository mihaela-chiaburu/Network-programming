using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.Interfaces
{
    public interface IMessageDisplay
    {
        void DisplayMessage(string message, bool isFromCurrentUser, StackPanel container, ScrollViewer scrollViewer);
        void ClearMessages(StackPanel container);
    }
}
