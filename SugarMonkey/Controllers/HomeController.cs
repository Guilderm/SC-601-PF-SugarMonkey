using System.Linq;
using System.Web.Mvc;
using SugarMonkey.DAL;
using SugarMonkey.Models.Old.Filters;
using SugarMonkey.Models.Old.Repository;

namespace SugarMonkey.Controllers
{
    [FrontPageActionFilter]
    public class HomeController : Controller
    {
        #region Other Class references ... // Instance on Unit of Work 

        public GenericUnitOfWork UnitOfWork = new GenericUnitOfWork();

        #endregion

        /// <summary>
        ///     Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.FeaturedProducts = UnitOfWork.GetRepositoryInstance<Tbl_Product>()
                .GetListByParameter(i => i.IsFeatured == true && i.IsDelete == false && i.IsActive == true).ToList();
            return View();
        }

        #region Disposing UnitOfWork Context ...

        protected override void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}