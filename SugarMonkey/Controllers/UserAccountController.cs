using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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


        public ActionResult Welcome()
        {
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string emailId)
        {
            string resetCode = Guid.NewGuid().ToString();
            string verifyUrl = "/Account/ResetPassword/" + resetCode;
            string link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                User getUser = (from s in dbContext.Users where s.Email == emailId select s).FirstOrDefault();
                if (getUser != null)
                {
                    //getUser.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    dbContext.Configuration.ValidateOnSaveEnabled = false;
                    dbContext.SaveChanges();

                    string subject = "Password Reset Request";
                    string body = "Hi " + getUser.FirstName +
                                  ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +
                                  " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                                  "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(getUser.Email, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }


        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id)) return HttpNotFound();

            using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
            {
                Credential credential = dbContext.Credentials.FirstOrDefault(a => a.ResetPasswordCode == id);
                if (credential == null) return HttpNotFound();
                ResetPassword model = new ResetPassword();
                model.ResetCode = id;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            string message = "";
            if (ModelState.IsValid)
                using (GeneralPurposeDBEntities dbContext = new GeneralPurposeDBEntities())
                {
                    Credential user = dbContext.Credentials
                        .FirstOrDefault(a => a.ResetPasswordCode == model.ResetCode);
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Password = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        dbContext.Configuration.ValidateOnSaveEnabled = false;
                        dbContext.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            else
                message = "Something invalid";

            ViewBag.Message = message;
            return View(model);
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
    }
}