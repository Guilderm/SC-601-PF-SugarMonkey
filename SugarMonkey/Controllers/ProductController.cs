using System.Web.Mvc;
using SugarMonkey.Models;
using SugarMonkey.Models.BusinessLogic;

namespace SugarMonkey.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductBusinessLogic modelo = new ProductBusinessLogic();

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

        [HttpGet]
        public ActionResult InsertProduct()
        {
            ViewBag.ProductFilter = modelo.ListaCategoryBag();
            ViewBag.ShowProduct = modelo.ListaProductsBag();

            return View();
        }

        [HttpPost]
        public ActionResult InsertProduct(Product obj, string Process)
        {
            ViewBag.ProductFilter = modelo.ListaCategoryBag();
            ViewBag.ShowProduct = modelo.ListaProductsBag();

            if (Process == "Ingresar")
            {
                modelo.InsertProduct(
                    obj.Name,
                    obj.Description,
                    obj.CategoryID,
                    obj.UnitPrice,
                    obj.ImagePath,
                    obj.ThumbnailPath,
                    obj.percentageOff,
                    obj.SaleStarts,
                    obj.SaleEnds);
            }
            else if (Process == "Actualizar")
            {
                modelo.UpdateProduct(obj);
            }

            return View();
        }
    }
}