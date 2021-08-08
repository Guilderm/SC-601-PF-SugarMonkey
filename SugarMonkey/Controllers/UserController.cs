using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SugarMonkey.Models;
using SugarMonkey.Models.BusinessLogic;
using SugarMonkey.Models.Views;

namespace SugarMonkey.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("index", "MainPage");
        }

        [HttpGet]
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(UserRegistrationViewModel userRegistrationViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "There is problem with the data";
                return View("UserRegistration", userRegistrationViewModel);
            }

            STP_CreateUser_Result userEntity = UserBusinessLogic.CreateUser(userRegistrationViewModel);
            ViewBag.Message = "El usuario fue creado exitosamente";
            return RedirectToAction("index", "MainPage");
        }

        [HttpGet]
        public ActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(LoginUserViewModel loginUserView)
        {
            if (!ModelState.IsValid)
            {
                return View(loginUserView);
            }

            STP_GetUserByCredentials_Result userEntity = UserBusinessLogic.GetUser(loginUserView);

            if (userEntity.UserID > 10)
            {
                Session["UserID"] = userEntity.UserID;
                //FormsAuthentication.SetAuthCookie(userEntity.UserID.ToString(), false);

                return RedirectToAction("index", "MainPage");
            }

            ModelState.AddModelError("Failure", "Wrong Username and password combination !");
            return View(loginUserView);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("index", "MainPage");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string eMail)
        {
            STP_SetResetPasswordCode_Result userEntity = UserBusinessLogic.SetResetPasswordCode(eMail);
            string verifyUrl = "/Account/ResetPasswordViewModel/" + userEntity.ResetPasswordCode;
            string link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            if (userEntity.UserID >= 10)
            {
                UserBusinessLogic.SendResetPasswordEmail(userEntity, link);
                ViewBag.Message = "Reset password link has been sent to your Email.";
                return View();
            }

            ViewBag.Message = "User doesn't exists.";
            return View();
        }


        [HttpGet]
        public ActionResult ResetPassword(string resetPasswordCode)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordCode))
            {
                return HttpNotFound();
            }

            ResetPasswordViewModel resetPasswordViewModel =
                UserBusinessLogic.GetUserByResetPasswordCode(resetPasswordCode);
            if (resetPasswordViewModel.UserID > 10)
            {
                return View(resetPasswordViewModel);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Something invalid";
                return View(resetPasswordViewModel);
            }

            STP_UpdateCredentials_Result userEntity = UserBusinessLogic.UpdateCredentials(resetPasswordViewModel);

            if (userEntity.UserID > 10)
            {
                ViewBag.Message = "New password updated successfully";
                return RedirectToAction("index", "MainPage");
            }

            ViewBag.Message = "Something invalid";
            return View(resetPasswordViewModel);
        }

        [HttpPost]
        public ActionResult UserEditInfo(EditUserViewModel editUserViewModel)
        {
            if (!ModelState.IsValid) return View("UserEditInfo", editUserViewModel);

            //TODO: Implement

            ViewBag.Message = "El usuario fue modificado exitosamente";
            return RedirectToAction("index", "MainPage");
        }
    }
}