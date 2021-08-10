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
            return RedirectToAction("index", "Main");
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
            return RedirectToAction("index", "Main");
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
                return RedirectToAction("index", "Main");
            }

            ModelState.AddModelError("Failure", "Wrong Username and password combination !");
            return View(loginUserView);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("index", "Main");
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
            if (resetPasswordViewModel.UserId > 10)
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
                return RedirectToAction("index", "Main");
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
                EditUserViewModel editUserViewModel = new EditUserViewModel();
                editUserViewModel.FirstName = userEntity.FirstName;
                editUserViewModel.FirstLastName = userEntity.FirstLastName;
                editUserViewModel.SecondLastName = userEntity.SecondLastName;
                editUserViewModel.Cellphone = (int)userEntity.Cellphone;
                editUserViewModel.Email = userEntity.Email;
                editUserViewModel.Password = userEntity.Password;

                return View(editUserViewModel);
            }

            return RedirectToAction("index", "Main");
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
            return RedirectToAction("index", "Main");
        }
    }
}