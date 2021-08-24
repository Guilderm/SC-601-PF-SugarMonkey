using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SugarMonkey.Models.BusinessLogic
{
    public class ProductBusinessLogic
    {
        public List<Product> ConsultarProducts(string category)
        {
            using (var context = new GeneralPurposeDBEntities())
            {
                var resultado = (from x in context.Products
                                 where (category == "0" ? true : x.CategoryID.ToString() == category)
                                 select x).ToList();
                return resultado;
            }
        }

        public List<SelectListItem> ListaCategoryBag()
        {
            List<SelectListItem> ListaProducts = new List<SelectListItem>();
            ListaProducts.Add(new SelectListItem { Value = "0", Text = "Todos" });
            ListaProducts.Add(new SelectListItem { Value = "100", Text = "Cupcakes" });
            ListaProducts.Add(new SelectListItem { Value = "101", Text = "Dulces" });
            ListaProducts.Add(new SelectListItem { Value = "102", Text = "Galletas" });
            ListaProducts.Add(new SelectListItem { Value = "103", Text = "Queques" });
            ListaProducts.Add(new SelectListItem { Value = "104", Text = "Reposteria" });

            return ListaProducts;
        }
    }
}