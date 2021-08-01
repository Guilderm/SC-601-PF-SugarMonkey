using System.Linq;
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

        public static int SetResetPasswordCode(string eMail, string ResetPasswordCode)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                int userId = dbContext.STP_SetResetPasswordCode(eMail, ResetPasswordCode).FirstOrDefault() ?? 0;
                return userId;
            }
        }

        public static int getUser(string eMail, string ResetPasswordCode)
        {
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                int userId = dbContext.STP_SetResetPasswordCode(eMail, ResetPasswordCode).FirstOrDefault() ?? 0;
                return userId;
            }
        }
        
    }
}