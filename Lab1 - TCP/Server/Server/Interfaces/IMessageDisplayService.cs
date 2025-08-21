using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Server.Interfaces
{
    public interface IMessageDisplayService
    {
        void DisplayMessage(string message, bool isFromServer, StackPanel container, ScrollViewer scrollViewer);
        void DisplaySystemMessage(string message, StackPanel container, ScrollViewer scrollViewer);
        void ClearMessages(StackPanel container);
    }
}
