using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
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


        public ActionResult Register()
        {
            return View();
        }

        //The form's data in Register view is posted to this method. 
        //We have binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new Entities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    RegisterUser reglog = new RegisterUser();

                    //Save all details in RegitserUser object

                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;


                    //Calling the SaveDetails method which saves the details.
                    databaseContext.RegisterUsers.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }

            //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
            return View("Register", registerDetails);
        }


        public ActionResult Login()
        {
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Login(Login model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {
                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null) return View("Welcome", isValidUser);

                //If the username and password combination is not present in DB then error message is shown.
                ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                return View();
            }

            //If model state is not valid, the model with error message is returned to the View.
            return View(model);
        }

        public ActionResult Welcome()
        {
            return View();
        }

        //function to check if User is valid or not
        public RegisterUser IsValidUser(Login model)
        {
            using (var dataContext = new LoginRegistrationInMVCEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers
                    .Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password))
                    .SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                return user;
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (var context = new LoginRegistrationInMVCEntities())
            {
                var getUser = (from s in context.RegisterUsers where s.Email == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.FirstName +
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

            using (var context = new LoginRegistrationInMVCEntities())
            {
                var user = context.RegisterUsers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }

                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
                using (var context = new LoginRegistrationInMVCEntities())
                {
                    var user = context.RegisterUsers.Where(a => a.ResetPasswordCode == model.ResetCode)
                        .FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Password = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
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
                NetworkCredential NetworkCred = new NetworkCredential("youremail@gmail.com", "YourPassword");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}