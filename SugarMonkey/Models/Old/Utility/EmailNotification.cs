using System.Configuration;
using System.Net.Mail;

namespace SugarMonkey.Models.Old.Utility
{
    public class EmailNotification
    {
        public static void SendMail(string recipientAddress, string emailSubject, string emailBody)
        {
            emailBody = emailBody.Replace("_RouteUrlPath", ConfigurationManager.AppSettings["ApplicationRootUrl"]);
            string[] emailRecipientList = recipientAddress.Split(',');
            SmtpClient smtp = new SmtpClient();
            MailMessage mail = new MailMessage();
            mail.Body = emailBody;
            mail.Subject = emailSubject;
            foreach (string user in emailRecipientList)
                mail.To.Add(new MailAddress(user));
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }
    }
}