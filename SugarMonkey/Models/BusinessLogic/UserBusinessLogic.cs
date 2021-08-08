using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using SugarMonkey.Models.Views;

namespace SugarMonkey.Models.BusinessLogic
{
    public class UserBusinessLogic
    {
        public static STP_CreateUser_Result CreateUser(UserRegistrationViewModel userRegistrationViewModel)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_CreateUser_Result userEntity = dbContext.STP_CreateUser(userRegistrationViewModel.FirstName,
                    userRegistrationViewModel.FirstLastName,
                    userRegistrationViewModel.SecondLastName,
                    userRegistrationViewModel.Cellphone,
                    userRegistrationViewModel.Email,
                    userRegistrationViewModel.Password,
                    "notSalted").FirstOrDefault();

                return userEntity;
            }
        }

        public static STP_GetUserByCredentials_Result GetUser(LoginUserViewModel LoginUserViewModel)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_GetUserByCredentials_Result userEntity = dbContext
                    .STP_GetUserByCredentials(LoginUserViewModel.Email, LoginUserViewModel.Password).FirstOrDefault();
                return userEntity;
            }
        }

        public static STP_GetUsersInfoByID_Result GetUserByID(int userID)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_GetUsersInfoByID_Result userEntity = dbContext
                    .STP_GetUsersInfoByID(userID).FirstOrDefault();
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
                ResetPasswordViewModel resetPassword = new ResetPasswordViewModel {UserID = userEntity.UserID};
                return resetPassword;
            }
        }

        public static STP_UpdateCredentials_Result UpdateCredentials(ResetPasswordViewModel resetPasswordViewModel)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_UpdateCredentials_Result userEntity = dbContext
                    .STP_UpdateCredentials(resetPasswordViewModel.UserID, resetPasswordViewModel.ConfirmPassword,
                        "not salted")
                    .FirstOrDefault();
                return userEntity;
            }
        }

        public static STP_UpdateCredentials_Result UpdateUser(EditUserViewModel editUserViewModel, int userID)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                STP_UpdateCredentials_Result userEntity = dbContext
                    .STP_UpdateUser(userID, editUserViewModel.FirstName, editUserViewModel.FirstLastName, editUserViewModel.SecondLastName,
                    editUserViewModel.Cellphone, editUserViewModel.Email, editUserViewModel.Password, "not salted")
                    .FirstOrDefault();
                return userEntity;
            }
        }
    }
}