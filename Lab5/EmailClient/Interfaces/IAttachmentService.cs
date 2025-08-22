using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient.Interfaces
{
    public interface IAttachmentService
    {
        Task<string> SelectAttachmentAsync();
        Task<int> DownloadAttachmentsAsync(MimeMessage message, string folderPath);
        Task<string> SelectDownloadFolderAsync();
    }
}
