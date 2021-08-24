using System.Linq;
using System.Web.Mvc;
using SugarMonkey.Controllers;
using SugarMonkey.DAL;
using SugarMonkey.Models.Old.Repository;

namespace SugarMonkey.Models.Old.Filters
{
    public class FrontPageActionFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dynamic controller;
            string controllerName = filterContext.RequestContext.HttpContext.Request.RawUrl.Split('/')[1].ToLower();
            switch (controllerName)
            {
                case "search":
                    controller = (SearchController) filterContext.Controller;
                    break;
                case "account":
                    controller = (UserController) filterContext.Controller;
                    break;
                case "admin":
                    controller = (AdminController) filterContext.Controller;
                    break;
                case "shopping":
                    controller = (ShoppingController) filterContext.Controller;
                    break;
                default:
                    controller = (MainPageController) filterContext.Controller;
                    break;
            }

            GenericUnitOfWork unitOfWork = controller._unitOfWork;
            filterContext.Controller.ViewBag.CategoryAndSubCategory = unitOfWork.GetRepositoryInstance<Tbl_Category>()
                .GetAllRecordsIQueryable().ToList();
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //  throw new System.NotImplementedException();
        }
    }
}