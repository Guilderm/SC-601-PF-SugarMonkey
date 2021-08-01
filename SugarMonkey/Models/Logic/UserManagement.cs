using System;
using System.Linq;
using System.Net.Mail;
using SugarMonkey.Models.View;
using SugarMonkey.Models.Entities;

using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using SugarMonkey.Models;
using SugarMonkey.Models.Entities;
using SugarMonkey.Models.Logic;
using SugarMonkey.Models.View;

namespace SugarMonkey.Models.Logic
{
    public class UserManagement
    {
        public static int? GetUserId(Login login)
        {
            return ValidateCredentials(login);
        }

        private static int ValidateCredentials(Login login)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                int userId = dbContext.STP_GetCredential(login.Email, login.Password).FirstOrDefault() ?? 0;
                return userId;
            }
        }

        public static STP_SetResetPasswordCode_Result SetResetPasswordCode(string eMail)
        {
            string resetPasswordCode = Guid.NewGuid().ToString(); 
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_SetResetPasswordCode_Result userEntity = dbContext.STP_SetResetPasswordCode(eMail, resetPasswordCode).FirstOrDefault();
                return userEntity;
            }
        }

        public static void SendResetPasswordEmail(STP_SetResetPasswordCode_Result userEntity, string link)
        {
            string subject = "Password Reset Request";
            string body = "Hi " + userEntity.FirstName +
                          ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +
                          " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                          "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";
            SendEmail(userEntity.Email, body, subject);
        }

        private static void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("youremail@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCred = new NetworkCredential("youremail@gmail.com", "YourPassword");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
} 