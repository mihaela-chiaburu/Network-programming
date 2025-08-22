using EmailClient.Constants;
using EmailClient.Interfaces;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Services
{
    public class AttachmentService : IAttachmentService
    {
        public async Task<string> SelectAttachmentAsync()
        {
            return await Task.Run(() =>
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    return dialog.FileName;
                }
                return null;
            });
        }

        public async Task<string> SelectDownloadFolderAsync()
        {
            return await Task.Run(() =>
            {
                var dialog = new SaveFileDialog
                {
                    Title = AppConstants.FileDialog.SELECT_FOLDER_TITLE,
                    FileName = AppConstants.FileDialog.SELECT_FOLDER_FILENAME,
                    Filter = AppConstants.FileDialog.FOLDER_FILTER,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    CheckFileExists = false,
                    CheckPathExists = true
                };

                if (dialog.ShowDialog() == true)
                {
                    return Path.GetDirectoryName(dialog.FileName);
                }
                return null;
            });
        }

        public async Task<int> DownloadAttachmentsAsync(MimeMessage message, string folderPath)
        {
            return await Task.Run(() =>
            {
                int count = 0;

                foreach (var attachment in message.Attachments)
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

                return count;
            });
        }
    }
}
