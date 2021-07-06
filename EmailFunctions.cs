using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ElectionManagementSystem
{
    class EmailFunctions
    {
        private static ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        //function sends emails
        public static bool SendMail(string RecipientMail, string RecipientName, string MailSubject, string MailBody)
        {
            bool status = false;
            var settings = _db.Settings.Where(s => s.Id == 1).SingleOrDefault();


            int Port = Convert.ToInt32(settings.SMTPPort);
            string Host = settings.SMTPHost;
            string Username = settings.GmailUsername;
            string Password = settings.GmailAppPassword;
            string SenderMail = settings.GmailUsername;
            string SenderName = settings.GmailUsername;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(settings.GmailUsername, settings.GmailAppPassword);
            client.Port = Port;
            client.Host = Host;
            client.EnableSsl = true;
            try
            {
                if (AppFunctions.IsInternetAvailable())
                {

                    using (MailMessage emailMessage = new MailMessage())
                    {
                        emailMessage.From = new MailAddress(SenderMail, SenderName);
                        emailMessage.To.Add(new MailAddress(RecipientMail, RecipientName));
                        emailMessage.Subject = MailSubject;
                        emailMessage.SubjectEncoding = Encoding.UTF8;
                        emailMessage.Body = MailBody;
                        emailMessage.IsBodyHtml = true;
                        emailMessage.BodyEncoding = Encoding.UTF8;
                        emailMessage.Priority = MailPriority.Normal;
                        using (SmtpClient MailClient = new SmtpClient(Host, Port))
                        {
                            MailClient.EnableSsl = true;
                            MailClient.Credentials = new System.Net.NetworkCredential(Username, Password);
                            MailClient.Send(emailMessage);
                        }
                    }
                    
                    status = true;
                }
            }
            catch (Exception e)
            {
                AppFunctions.WriteLog(e.Message);
            }

            return status;
        }
        //function sends emails with attachment
        public static bool SendMailAttachemnt(string RecipientMail, string RecipientName, string MailSubject, string MailBody, string file)
        {            
            bool status = false;
            var settings = _db.Settings.Where(s => s.Id == 1).SingleOrDefault();


            int Port = Convert.ToInt32(settings.SMTPPort);
            string Host = settings.SMTPHost;
            string Username = settings.GmailUsername;
            string Password = settings.GmailAppPassword;
            string SenderMail = settings.GmailUsername;
            string SenderName = settings.GmailUsername;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(settings.GmailUsername, settings.GmailAppPassword);
            client.Port = Port;
            client.Host = Host;
            client.EnableSsl = true;
            try
            {
                if (AppFunctions.IsInternetAvailable())
                {

                    using (MailMessage emailMessage = new MailMessage())
                    {
                        emailMessage.From = new MailAddress(SenderMail, SenderName);
                        emailMessage.To.Add(new MailAddress(RecipientMail, RecipientName));
                        emailMessage.Subject = MailSubject;
                        emailMessage.SubjectEncoding = Encoding.UTF8;
                        emailMessage.Body = MailBody;
                        emailMessage.IsBodyHtml = true;
                        emailMessage.BodyEncoding = Encoding.UTF8;
                        emailMessage.Priority = MailPriority.Normal;

                        // Create  the file attachment for this e-mail message.
                        Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                        // Add time stamp information for the file.
                        ContentDisposition disposition = data.ContentDisposition;
                        disposition.CreationDate = System.IO.File.GetCreationTime(file);
                        disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                        disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                        // Add the file attachment to this e-mail message.
                        emailMessage.Attachments.Add(data);
                        emailMessage.Attachments.Add(data);


                        using (SmtpClient MailClient = new SmtpClient(Host, Port))
                        {
                            MailClient.EnableSsl = true;
                            MailClient.Credentials = new System.Net.NetworkCredential(settings.GmailUsername, settings.GmailAppPassword);
                            MailClient.Send(emailMessage);
                        }
                    }
                    
                    status = true;
                }
            }
            catch (Exception e)
            {
                AppFunctions.WriteLog(e.Message);
            }

            return status;
        }
        
    }
}
