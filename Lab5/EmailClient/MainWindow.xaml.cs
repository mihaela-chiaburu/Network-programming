using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Search;
using MimeKit;
using Attachment = System.Net.Mail.Attachment;

namespace EmailClient
{
    public partial class MainWindow : Window
    {
        private List<MimeMessage> emailMessages = new List<MimeMessage>();
        private List<string> attachments = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComposeButton_Click(object sender, RoutedEventArgs e)
        {
            emailsListView.Visibility = Visibility.Collapsed;
            emailContentGrid.Visibility = Visibility.Collapsed;
            composeEmailGrid.Visibility = Visibility.Visible;

            replyToTextBox.Text = emailTextBox.Text;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(emailTextBox.Text) || string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Please enter your email and password");
                return;
            }

            try
            {
                statusTextBlock.Text = "Connecting to server...";

                emailMessages.Clear();
                emailsListView.ItemsSource = null;

                if (pop3Radio.IsChecked == true)
                {
                    await LoadEmailsViaPop3();
                }
                else
                {
                    await LoadEmailsViaImap();
                }

                var emailList = emailMessages.Select(m => new
                {
                    From = m.From.ToString(),
                    Subject = m.Subject,
                    Date = m.Date.LocalDateTime.ToString("g"),
                    Message = m
                }).ToList();

                emailsListView.ItemsSource = emailList;
                emailsListView.Visibility = Visibility.Visible;
                emailContentGrid.Visibility = Visibility.Collapsed;
                composeEmailGrid.Visibility = Visibility.Collapsed;

                statusTextBlock.Text = $"Loaded {emailMessages.Count} emails";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading emails: {ex.Message}");
                statusTextBlock.Text = "Error loading emails";
            }
        }

        private async Task LoadEmailsViaPop3()
        {
            using (var client = new Pop3Client())
            {
                await client.ConnectAsync(pop3ServerTextBox.Text, int.Parse(pop3PortTextBox.Text), true);
                await client.AuthenticateAsync(emailTextBox.Text, passwordBox.Password);

                for (int i = 0; i < client.Count; i++)
                {
                    var message = await client.GetMessageAsync(i);
                    emailMessages.Add(message);
                }

                await client.DisconnectAsync(true);
            }
        }

        private async Task LoadEmailsViaImap()
        {
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(imapServerTextBox.Text, int.Parse(imapPortTextBox.Text), true);
                await client.AuthenticateAsync(emailTextBox.Text, passwordBox.Password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                // doar ultimele 15 email-uri 
                var count = Math.Min(inbox.Count, 15); 
                var uids = await inbox.SearchAsync(SearchQuery.All);
                var latestUids = uids.TakeLast(count).Reverse().ToList(); 

                emailMessages.Clear();

                foreach (var uid in latestUids)
                {
                    var message = await inbox.GetMessageAsync(uid);
                    emailMessages.Add(message);
                }

                await client.DisconnectAsync(true);
            }
        }

        private void EmailsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (emailsListView.SelectedItem == null) return;

            dynamic selectedItem = emailsListView.SelectedItem;
            var selectedMessage = (MimeMessage)selectedItem.Message;

            // Display email
            emailContentGrid.DataContext = new
            {
                SelectedEmail = new
                {
                    From = selectedMessage.From.ToString(),
                    Subject = selectedMessage.Subject,
                    Date = selectedMessage.Date.LocalDateTime.ToString("g")
                }
            };

            // Display HTML
            var html = selectedMessage.HtmlBody;
            if (!string.IsNullOrEmpty(html))
            {
                emailWebBrowser.NavigateToString(html);
            }
            else
            {
                emailWebBrowser.NavigateToString($"<pre>{selectedMessage.TextBody}</pre>");
            }

            emailsListView.Visibility = Visibility.Collapsed;
            emailContentGrid.Visibility = Visibility.Visible;
            composeEmailGrid.Visibility = Visibility.Collapsed;
        }

        private void CloseEmailButton_Click(object sender, RoutedEventArgs e)
        {
            emailsListView.Visibility = Visibility.Visible;
            emailContentGrid.Visibility = Visibility.Collapsed;
            composeEmailGrid.Visibility = Visibility.Collapsed;
        }

        private void AddAttachment_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                attachments.Add(dialog.FileName);
                statusTextBlock.Text = $"Added attachment: {Path.GetFileName(dialog.FileName)}";
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusTextBlock.Text = "Sending email...";

                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailTextBox.Text, passwordBox.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailTextBox.Text),
                    Subject = subjectTextBox.Text,
                    Body = bodyTextBox.Text,
                    IsBodyHtml = false
                };

                if (!string.IsNullOrEmpty(replyToTextBox.Text))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(replyToTextBox.Text));
                }

                mailMessage.To.Add(toTextBox.Text);

                foreach (var attachmentPath in attachments)
                {
                    if (File.Exists(attachmentPath))
                    {
                        var attachment = new Attachment(attachmentPath);
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                smtpClient.Send(mailMessage);

                toTextBox.Clear();
                subjectTextBox.Clear();
                bodyTextBox.Clear();
                attachments.Clear();

                emailsListView.Visibility = Visibility.Visible;
                emailContentGrid.Visibility = Visibility.Collapsed;
                composeEmailGrid.Visibility = Visibility.Collapsed;

                statusTextBlock.Text = "Email sent successfully";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}\n\nMake sure you've enabled 'Less secure app access' or created an App Password in your Google account settings.");
                statusTextBlock.Text = "Error sending email";
            }
        }

        private void CancelComposeButton_Click(object sender, RoutedEventArgs e)
        {
            emailsListView.Visibility = Visibility.Visible;
            emailContentGrid.Visibility = Visibility.Collapsed;
            composeEmailGrid.Visibility = Visibility.Collapsed;

            toTextBox.Clear();
            subjectTextBox.Clear();
            bodyTextBox.Clear();
            attachments.Clear();
        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (emailsListView.SelectedItem == null) return;

            dynamic selectedItem = emailsListView.SelectedItem;
            var selectedMessage = (MimeMessage)selectedItem.Message;

            emailsListView.Visibility = Visibility.Collapsed;
            emailContentGrid.Visibility = Visibility.Collapsed;
            composeEmailGrid.Visibility = Visibility.Visible;

            // Pre-fill reply
            toTextBox.Text = selectedMessage.From.ToString();
            subjectTextBox.Text = $"Re: {selectedMessage.Subject}";
            replyToTextBox.Text = emailTextBox.Text;
            bodyTextBox.Text = $"\n\n---------- Original Message ----------\n{selectedMessage.TextBody}";
        }

        private void DownloadAttachments_Click(object sender, RoutedEventArgs e)
        {
            if (emailsListView.SelectedItem == null) return;

            dynamic selectedItem = emailsListView.SelectedItem;
            var selectedMessage = (MimeMessage)selectedItem.Message;

            // Create a SaveFileDialog to get a folder path
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Select folder to save attachments",
                FileName = "SelectFolder",
                Filter = "Folders|*.thisisnotafile",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CheckFileExists = false,
                CheckPathExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(dialog.FileName);
                int count = 0;

                foreach (var attachment in selectedMessage.Attachments)
                {
                    var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = File.Create(filePath))
                    {
                        if (attachment is MimePart part)
                        {
                            part.Content.DecodeTo(stream);
                        }
                        else
                        {
                            var rfc822 = (MessagePart)attachment;
                            rfc822.Message.WriteTo(stream);
                        }
                    }
                    count++;
                }

                statusTextBlock.Text = $"Downloaded {count} attachments";
            }
        }
    }
}