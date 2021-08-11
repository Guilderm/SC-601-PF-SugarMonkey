using System.Configuration;
using System.Net.Mail;

namespace SugarMonkey.Utility
{
    public class EmailNotification
    {
        public static void SendMail(string recipientAddress, string emailSubject, string emailBody)
        {
            emailBody = emailBody.Replace("_RouteUrlPath", ConfigurationManager.AppSettings["ApplicationRootUrl"]);
            var EmailRecipientList = recipientAddress.Split(',');
            SmtpClient smtp = new SmtpClient();
            MailMessage mail = new MailMessage();
            mail.Body = emailBody;
            mail.Subject = emailSubject;
            foreach (var user in EmailRecipientList)
                mail.To.Add(new MailAddress(user));
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }
    }
}