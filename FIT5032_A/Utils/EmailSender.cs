using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FIT5032_A.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FIT5032_A.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "";

        
        public void Send(String toEmail, String subject, String contents, String filePath, String fileExtention)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("", "FIT5032 Example Email User");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(filePath));
            msg.Attachments = new List<Attachment>
        {
            new SendGrid.Helpers.Mail.Attachment
            {
                Content = Convert.ToBase64String(byteData),
                Filename = filePath,
                Type = filePath.Substring(filePath.LastIndexOf(".")+1),
                Disposition = "attachment"
            }
        };
            var response = client.SendEmailAsync(msg);
        }
        public void Send_To_Many(List<EmailAddress> toEmails, String subject, String contents, String filePath)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("", "FIT5032 Example Email User");
            var tos = toEmails;
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var showAllRecipients = false;
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent, showAllRecipients);
            byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(filePath));
            msg.Attachments = new List<Attachment>
        {
            new SendGrid.Helpers.Mail.Attachment
            {
                Content = Convert.ToBase64String(byteData),
                Filename = filePath,
                Type = filePath.Substring(filePath.LastIndexOf(".")+1),
                Disposition = "attachment"
            }
        };
            var response = client.SendEmailAsync(msg);
        }

    }
}
