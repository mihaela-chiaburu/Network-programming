using DNS_Client.Constants;
using DNS_Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DNS_Client.Services
{
    public class OutputService : IOutputService
    {
        public void AppendOutput(string text, TextBox outputTextBox)
        {
            AppendText(text, outputTextBox, Brushes.Black);
        }

        public void AppendSuccess(string text, TextBox outputTextBox)
        {
            AppendText(text, outputTextBox, Brushes.DarkGreen);
        }

        public void AppendError(string text, TextBox outputTextBox)
        {
            AppendText(text, outputTextBox, Brushes.DarkRed);
        }

        public void AppendInfo(string text, TextBox outputTextBox)
        {
            AppendText(text, outputTextBox, Brushes.DarkBlue);
        }

        private void AppendText(string text, TextBox outputTextBox, Brush color)
        {
            if (outputTextBox == null) return;

            outputTextBox.Dispatcher.Invoke(() =>
            {
                outputTextBox.Foreground = color;
                outputTextBox.AppendText(text + AppConstants.UI.OUTPUT_SEPARATOR);
                outputTextBox.ScrollToEnd();
            });
        }

        public void Clear(TextBox outputTextBox)
        {
            if (outputTextBox == null) return;

            outputTextBox.Dispatcher.Invoke(() =>
            {
                outputTextBox.Clear();
            });
        }
    }
}
