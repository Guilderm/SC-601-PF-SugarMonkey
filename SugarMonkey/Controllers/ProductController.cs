using SugarMonkey.Models;
using SugarMonkey.Models.BusinessLogic;
using System.Web.Mvc;

namespace SugarMonkey.Controllers
{
    public class ProductController : Controller
    {
        ProductBusinessLogic modelo = new ProductBusinessLogic();

        [HttpGet]
        public ActionResult Product()
        {
            ViewBag.ProductFilter = modelo.ListaCategoryBag();

            return View(modelo.ConsultarProducts("0"));
        }

        [HttpPost]
        public ActionResult ConsultarProducts(Product obj)
        {
            ViewBag.ProductFilter = modelo.ListaCategoryBag();

            return View("Product", modelo.ConsultarProducts(obj.CategoryID.ToString()));
        }
    }
}