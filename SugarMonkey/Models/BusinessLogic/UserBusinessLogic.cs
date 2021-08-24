using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using SugarMonkey.Models.Views;

namespace SugarMonkey.Models.BusinessLogic
{
    public class UserBusinessLogic
    {
        public static STP_CreateUser_Result CreateUser(UserRegistrationEditViewModel userRegistrationEditViewModel)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_CreateUser_Result userEntity = dbContext.STP_CreateUser(userRegistrationEditViewModel.FirstName,
                    userRegistrationEditViewModel.FirstLastName,
                    userRegistrationEditViewModel.SecondLastName,
                    userRegistrationEditViewModel.Cellphone,
                    userRegistrationEditViewModel.Email,
                    userRegistrationEditViewModel.Password,
                    "notSalted").FirstOrDefault();

                return userEntity;
            }
        }


        public static STP_GetUsersInfoByID_Result GetUserById(int userId)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_GetUsersInfoByID_Result userEntity = dbContext
                    .STP_GetUsersInfoByID(userId).FirstOrDefault();
                return userEntity;
            }
        }


        public static STP_SetResetPasswordCode_Result SetResetPasswordCode(string eMail)
        {
            string resetPasswordCode = Guid.NewGuid().ToString();
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_SetResetPasswordCode_Result userEntity =
                    dbContext.STP_SetResetPasswordCode(eMail, resetPasswordCode).FirstOrDefault();
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

        public static ResetPasswordViewModel GetUserByResetPasswordCode(string resetPasswordCode)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_GetUserByResetPasswordCode_Result userEntity =
                    dbContext.STP_GetUserByResetPasswordCode(resetPasswordCode).FirstOrDefault();
                ResetPasswordViewModel resetPassword = new ResetPasswordViewModel {UserId = userEntity.UserID};
                return resetPassword;
            }
        }

        public static STP_UpdateCredentials_Result UpdateCredentials(ResetPasswordViewModel resetPasswordViewModel)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_UpdateCredentials_Result userEntity = dbContext
                    .STP_UpdateCredentials(resetPasswordViewModel.UserId, resetPasswordViewModel.ConfirmPassword,
                        "not salted")
                    .FirstOrDefault();
                return userEntity;
            }
        }

        public static STP_UpdateUser_Result UpdateUser(EditUserViewModel editUserViewModel, int userId)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_UpdateUser_Result userEntity = dbContext.STP_UpdateUser(userId, editUserViewModel.FirstName,
                        editUserViewModel.FirstLastName, editUserViewModel.SecondLastName, editUserViewModel.Cellphone,
                        editUserViewModel.Email, editUserViewModel.Password, "not salted")
                    .FirstOrDefault();
                return userEntity;
            }
        }
    }
}