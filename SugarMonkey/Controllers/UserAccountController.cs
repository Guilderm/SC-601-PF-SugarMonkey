using System.Web.Mvc;
using System.Web.Security;
using SugarMonkey.Models;
using SugarMonkey.Models.Logic;
using SugarMonkey.Models.View;

namespace SugarMonkey.Controllers
{
    public class UserAccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
                {
                    dbContext.STP_CreateUser(userRegistration.FirstName,
                        userRegistration.FirstLastName,
                        userRegistration.SecondLastName,
                        userRegistration.Cellphone,
                        userRegistration.Email,
                        userRegistration.Password,
                        "notSalted");
                }

                ViewBag.Message = "El usuario fue creado exitosamente";
                return RedirectToAction("index", "Home");
            }

            return View("UserRegistration", userRegistration);
        }

        [HttpGet]
        public ActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            int userId = (int) UserManagement.GetUserId(login);

            if (userId > 0)
            {
                Session["UserID"] = userId;
                return RedirectToAction("index", "Home");
            }

            ModelState.AddModelError("Failure", "Wrong Username and password combination !");
            return View(login);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("index", "Home");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string eMail)
        {
            STP_SetResetPasswordCode_Result userEntity = UserManagement.SetResetPasswordCode(eMail);
            string verifyUrl = "/Account/ResetPasswordView/" + userEntity.ResetPasswordCode;
            string link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            if (userEntity.UserID >= 10)
            {
                UserManagement.SendResetPasswordEmail(userEntity, link);
                ViewBag.Message = "Reset password link has been sent to your Email.";

                return View();
            }

            ViewBag.Message = "User doesn't exists.";

            return View();
        }


        [HttpGet]
        public ActionResult ResetPassword(string ResetPasswordCode)
        {
            if (string.IsNullOrWhiteSpace(ResetPasswordCode))
            {
                return HttpNotFound();
            }

            ;
            ResetPasswordView resetPasswordView = UserManagement.GetUserByResetPasswordCode(ResetPasswordCode);
            if (resetPasswordView.UserID > 10)
            {
                return HttpNotFound();
            }

            return View(resetPasswordView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordView resetPasswordView)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Something invalid";
                return View(resetPasswordView);
            }

            STP_UpdateCredentials_Result userEntity = UserManagement.UpdateCredentials(resetPasswordView);

            if (userEntity.UserID > 10)
            {
                ViewBag.Message = "New password updated successfully";
                return RedirectToAction("index", "Home");
            }

            ViewBag.Message = "Something invalid";
            return View(resetPasswordView);
        }

        private void SendEmail(string emailAddress, string body, string subject)
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

        [HttpPost]
        public ActionResult UserEditInfo(UserEditInfo userEditInfo)
        {
            if (ModelState.IsValid)
            {
                using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
                {
                    dbContext.STP_GetUsersInfoByEmail(userEditInfo.Email);
                }

                ViewBag.Message = "El usuario fue modificado exitosamente";
                return RedirectToAction("index", "Home");
            }

            return View("UserEditInfo", userEditInfo);
        }
    }
}