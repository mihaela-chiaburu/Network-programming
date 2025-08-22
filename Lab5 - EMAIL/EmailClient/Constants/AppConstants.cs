using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Constants
{
    public static class AppConstants
    {
        public static class Smtp
        {
            public const string GMAIL_SERVER = "smtp.gmail.com";
            public const int GMAIL_PORT = 587;
            public const bool USE_SSL = true;
        }

        public static class Email
        {
            public const int MAX_EMAILS_TO_LOAD = 15;
            public const string REPLY_PREFIX = "Re: ";
            public const string ORIGINAL_MESSAGE_HEADER = "\n\n---------- Original Message ----------\n";
        }

        public static class FileDialog
        {
            public const string FOLDER_FILTER = "Folders|*.thisisnotafile";
            public const string SELECT_FOLDER_TITLE = "Select folder to save attachments";
            public const string SELECT_FOLDER_FILENAME = "SelectFolder";
        }

        public static class Messages
        {
            public const string ENTER_EMAIL_PASSWORD = "Please enter your email and password";
            public const string CONNECTING_TO_SERVER = "Connecting to server...";
            public const string SENDING_EMAIL = "Sending email...";
            public const string EMAIL_SENT_SUCCESS = "Email sent successfully";
            public const string ERROR_LOADING_EMAILS = "Error loading emails: {0}";
            public const string ERROR_SENDING_EMAIL = "Error sending email: {0}\n\nMake sure you've enabled 'Less secure app access' or created an App Password in your Google account settings.";
            public const string ADDED_ATTACHMENT = "Added attachment: {0}";
            public const string LOADED_EMAILS_COUNT = "Loaded {0} emails";
            public const string DOWNLOADED_ATTACHMENTS_COUNT = "Downloaded {0} attachments";
            public const string ERROR_LOADING_EMAILS_STATUS = "Error loading emails";
            public const string ERROR_SENDING_EMAIL_STATUS = "Error sending email";
        }
    }
}
