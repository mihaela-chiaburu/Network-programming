using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DNS_Client.Interfaces
{
    public interface IOutputService
    {
        void AppendOutput(string text, TextBox outputTextBox);
        void AppendSuccess(string text, TextBox outputTextBox);
        void AppendError(string text, TextBox outputTextBox);
        void AppendInfo(string text, TextBox outputTextBox);
        void Clear(TextBox outputTextBox);
    }
}
