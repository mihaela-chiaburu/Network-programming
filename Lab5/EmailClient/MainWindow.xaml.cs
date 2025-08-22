using EmailClient.Constants;
using EmailClient.Interfaces;
using EmailClient.Models;
using EmailClient.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EmailClient
{
    public partial class MainWindow : Window
    {
        private IEmailSendingService _emailSendingService;
        private IAttachmentService _attachmentService;
        private IEmailContentService _emailContentService;

        private List<MimeMessage> _emailMessages = new List<MimeMessage>();
        private List<string> _attachments = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            _emailSendingService = new SmtpEmailService();
            _attachmentService = new AttachmentService();
            _emailContentService = new EmailContentService();
        }

        // ---- UI ----
        private void ComposeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowComposeView();
            replyToTextBox.Text = emailTextBox.Text;
        }

        private void CloseEmailButton_Click(object sender, RoutedEventArgs e)
        {
            ShowEmailListView();
        }

        private void CancelComposeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowEmailListView();
            ClearComposeForm();
        }

        private void ShowEmailListView()
        {
            emailsListView.Visibility = Visibility.Visible;
            emailContentGrid.Visibility = Visibility.Collapsed;
            composeEmailGrid.Visibility = Visibility.Collapsed;
        }

        private void ShowEmailContentView()
        {
            emailsListView.Visibility = Visibility.Collapsed;
            emailContentGrid.Visibility = Visibility.Visible;
            composeEmailGrid.Visibility = Visibility.Collapsed;
        }

        private void ShowComposeView()
        {
            emailsListView.Visibility = Visibility.Collapsed;
            emailContentGrid.Visibility = Visibility.Collapsed;
            composeEmailGrid.Visibility = Visibility.Visible;
        }

        // ---- EMAIL LOADING ----
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var connectionInfo = GetConnectionInfo();
            if (connectionInfo == null) return;

            try
            {
                UpdateStatus(AppConstants.Messages.CONNECTING_TO_SERVER);
                await LoadEmails(connectionInfo);
                DisplayEmailList();
                UpdateStatus(string.Format(AppConstants.Messages.LOADED_EMAILS_COUNT, _emailMessages.Count));
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_LOADING_EMAILS, ex.Message));
                UpdateStatus(AppConstants.Messages.ERROR_LOADING_EMAILS_STATUS);
            }
        }

        private async Task LoadEmails(EmailConnectionInfo connectionInfo)
        {
            _emailMessages.Clear();
            emailsListView.ItemsSource = null;

            var emailRetrievalService = EmailServiceFactory.CreateEmailRetrievalService(connectionInfo);
            _emailMessages = await emailRetrievalService.LoadEmailsAsync(connectionInfo);
        }

        private void DisplayEmailList()
        {
            var emailList = _emailMessages.Select(m => new EmailMessage(m)).ToList();
            emailsListView.ItemsSource = emailList;
            ShowEmailListView();
        }

        // ---- EMAIL DISPLAY ----
        private void EmailsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (emailsListView.SelectedItem == null) return;

            var selectedEmail = (EmailMessage)emailsListView.SelectedItem;
            var displayInfo = _emailContentService.PrepareEmailForDisplay(selectedEmail.Message);

            DisplayEmailContent(displayInfo);
            ShowEmailContentView();
        }

        private void DisplayEmailContent(EmailDisplayInfo displayInfo)
        {
            emailContentGrid.DataContext = new
            {
                SelectedEmail = new
                {
                    From = displayInfo.From,
                    Subject = displayInfo.Subject,
                    Date = displayInfo.Date
                }
            };

            emailWebBrowser.NavigateToString(displayInfo.HtmlContent);
        }

        // ---- EMAIL COMPOSITION ----
        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (emailsListView.SelectedItem == null) return;

            var selectedEmail = (EmailMessage)emailsListView.SelectedItem;
            var replyData = _emailContentService.PrepareReplyData(selectedEmail.Message, emailTextBox.Text);

            PopulateComposeForm(replyData);
            ShowComposeView();
        }

        private void PopulateComposeForm(ComposeEmailData composeData)
        {
            toTextBox.Text = composeData.To;
            subjectTextBox.Text = composeData.Subject;
            replyToTextBox.Text = composeData.ReplyTo;
            bodyTextBox.Text = composeData.Body;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var connectionInfo = GetConnectionInfo();
            var emailData = GetComposeEmailData();

            if (connectionInfo == null || emailData == null) return;

            try
            {
                UpdateStatus(AppConstants.Messages.SENDING_EMAIL);
                await _emailSendingService.SendEmailAsync(connectionInfo, emailData);

                ClearComposeForm();
                ShowEmailListView();
                UpdateStatus(AppConstants.Messages.EMAIL_SENT_SUCCESS);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(string.Format(AppConstants.Messages.ERROR_SENDING_EMAIL, ex.Message));
                UpdateStatus(AppConstants.Messages.ERROR_SENDING_EMAIL_STATUS);
            }
        }

        // ---- ATTACHMENTS ----
        private async void AddAttachment_Click(object sender, RoutedEventArgs e)
        {
            var filePath = await _attachmentService.SelectAttachmentAsync();
            if (!string.IsNullOrEmpty(filePath))
            {
                _attachments.Add(filePath);
                UpdateStatus(string.Format(AppConstants.Messages.ADDED_ATTACHMENT, Path.GetFileName(filePath)));
            }
        }

        private async void DownloadAttachments_Click(object sender, RoutedEventArgs e)
        {
            if (emailsListView.SelectedItem == null) return;

            var selectedEmail = (EmailMessage)emailsListView.SelectedItem;
            var folderPath = await _attachmentService.SelectDownloadFolderAsync();

            if (!string.IsNullOrEmpty(folderPath))
            {
                var count = await _attachmentService.DownloadAttachmentsAsync(selectedEmail.Message, folderPath);
                UpdateStatus(string.Format(AppConstants.Messages.DOWNLOADED_ATTACHMENTS_COUNT, count));
            }
        }

        private EmailConnectionInfo GetConnectionInfo()
        {
            if (string.IsNullOrEmpty(emailTextBox.Text) || string.IsNullOrEmpty(passwordBox.Password))
            {
                ShowWarningMessage(AppConstants.Messages.ENTER_EMAIL_PASSWORD);
                return null;
            }

            return new EmailConnectionInfo
            {
                Email = emailTextBox.Text,
                Password = passwordBox.Password,
                ImapServer = imapServerTextBox.Text,
                ImapPort = int.Parse(imapPortTextBox.Text),
                Pop3Server = pop3ServerTextBox.Text,
                Pop3Port = int.Parse(pop3PortTextBox.Text),
                UseImap = imapRadio.IsChecked == true
            };
        }

        private ComposeEmailData GetComposeEmailData()
        {
            return new ComposeEmailData
            {
                To = toTextBox.Text,
                Subject = subjectTextBox.Text,
                Body = bodyTextBox.Text,
                ReplyTo = replyToTextBox.Text,
                Attachments = new List<string>(_attachments)
            };
        }

        private void ClearComposeForm()
        {
            toTextBox.Clear();
            subjectTextBox.Clear();
            bodyTextBox.Clear();
            _attachments.Clear();
        }

        private void UpdateStatus(string message)
        {
            statusTextBlock.Text = message;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}