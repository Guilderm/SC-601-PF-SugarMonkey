using System;
using System.Linq;
using System.Web.Mvc;
using SugarMonkey.Models;
using SugarMonkey.Models.BusinessLogic;
using SugarMonkey.Models.Old.Repository;
using SugarMonkey.Models.Old.Utility;
using SugarMonkey.Models.Views;

namespace SugarMonkey.Controllers
{

    public class UserController : Controller
    {
        private readonly GeneralPurposeDBEntities _dbContext = new GeneralPurposeDBEntities();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogUserIn()
        {
            if (IsUserLogIn())
            {
                return RedirectToAction("Index", "MainPage");
            }

            LogUserInViewModel logUserIn = new LogUserInViewModel
            {
                Email = Request.Cookies["RememberMe_UserEmailId"] != null
                    ? Request.Cookies["RememberMe_UserEmailId"].Value
                    : "",
                Password = Request.Cookies["RememberMe_Password"] != null
                    ? Request.Cookies["RememberMe_Password"].Value
                    : ""
            };
            return View(logUserIn);
        }

        private bool IsUserLogIn()
        {
            bool alreadyLoggedIn = Session["MemberId"] != null;
            return alreadyLoggedIn;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult LogUserIn(LogUserInViewModel logUserIn)
        {
            //if (!ModelState.IsValid) return View(LogUserIn);
            STP_GetUserByCredentials_Result user =
                _dbContext.STP_GetUserByCredentials(logUserIn.Email, logUserIn.Password).FirstOrDefault();
            if (user != null)
            {
                Session["MemberId"] = user.UserID;
                Session["MemberName"] = user.FirstName;
                Session["MemberRole"] = user.UserType;

                if (Request["RememberMe1"] == "on")
                {
                    Response.Cookies["RememberMe_UserEmailId"].Value = user.Email;
                    Response.Cookies["RememberMe_Password"].Value = user.Password;
                }
                else
                {
                    Response.Cookies["RememberMe_UserEmailId"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["RememberMe_Password"].Expires = DateTime.Now.AddDays(-1);
                }

                return RedirectToAction("Index", "MainPage");
            }

            ModelState.AddModelError("Password", "Invalid email address or password");

            return View(logUserIn);
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            if (Request.Cookies["MemberRole"] != null)
                Response.Cookies["MemberRole"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "MainPage");
        }

        public JsonResult CheckIfEmailExist(string userEmailId)
        {
            //TODO : Needs to be implemented
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResetPassword(string emailId)
        {
            ResetPasswordViewModel rpm = new ResetPasswordViewModel();
            if (emailId != null)
            {
                try
                {
                    ModelState.Clear();
                    rpm.EmailId = EncryptDecrypt.Decrypt(emailId, true);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Invalid email address");
                }
            }
            else
                ModelState.AddModelError("", "Invalid email address");

            return View(rpm);
        }

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
        public ActionResult UserRegistration(UserRegistrationEditViewModel userRegistrationEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "There is problem with the data";
                return View("UserRegistration", userRegistrationEditViewModel);
            }

            STP_CreateUser_Result userEntity = UserBusinessLogic.CreateUser(userRegistrationEditViewModel);
            ViewBag.Message = "El usuario fue creado exitosamente";
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

        [HttpGet]
        public ActionResult UserEditInfo()
        {
            // Carga la informacion de usuario basado en el ID de la sesion
            int userId = int.Parse(Session["UserID"].ToString());
            STP_GetUsersInfoByID_Result userEntity = UserBusinessLogic.GetUserById(userId);

            if (userEntity.UserID > 10)
            {
                // Llena el modelo de EditUserViewModel con la informacion cargada
                EditUserViewModel editUserViewModel = new EditUserViewModel
                {
                    FirstName = userEntity.FirstName,
                    FirstLastName = userEntity.FirstLastName,
                    SecondLastName = userEntity.SecondLastName,
                    Cellphone = (int)userEntity.Cellphone,
                    Email = userEntity.Email,
                    Password = userEntity.Password
                };

                return View(editUserViewModel);
            }

            return RedirectToAction("index", "MainPage");
        }

        [HttpPost]
        public ActionResult UserEditInfo(EditUserViewModel editUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "There is problem with the data";
                return View("UserEditInfo", editUserViewModel);
            }

            if (!ModelState.IsValid) return View("UserEditInfo", editUserViewModel);

            // Carga la informacion de usuario basado en el ID de la sesion
            int userId = int.Parse(Session["UserID"].ToString());
            STP_UpdateUser_Result userEntity = UserBusinessLogic.UpdateUser(editUserViewModel, userId);
            ViewBag.Message = "El usuario fue creado exitosamente";
            //TODO: Implement

            ViewBag.Message = "El usuario fue modificado exitosamente";
            return RedirectToAction("index", "MainPage");
        }
    }
}