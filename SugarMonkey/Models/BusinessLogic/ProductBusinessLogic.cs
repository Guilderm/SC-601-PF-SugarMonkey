using System;
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

        public List<SelectListItem> ListaProductsBag()
        {
            List<SelectListItem> ListaProducts = new List<SelectListItem>();
            using (var context = new GeneralPurposeDBEntities())
            {
                var resultado = (from x in context.Products
                                 select x).ToList();

                ListaProducts.Add(new SelectListItem { Value = "0", Text = "Seleccione..." });
                foreach (var item in resultado)
                {
                    ListaProducts.Add(new SelectListItem { Value = item.ProductID.ToString(), Text = item.Name });
                }
                return ListaProducts;
            }
        }

        public void InsertProduct(string name, string description, int CategoryID, decimal price, string imagePath, string thumbnail, int discount, DateTime starts, DateTime ends)
        {
            using (var context = new GeneralPurposeDBEntities())
            {
                Product obj = new Product();
                obj.Name = name;
                obj.Description = description;
                obj.CategoryID = CategoryID;
                obj.UnitPrice = price;
                obj.ImagePath = imagePath;
                obj.ThumbnailPath = thumbnail;
                obj.percentageOff = discount;
                obj.SaleStarts = starts;
                obj.SaleEnds = ends;
                context.Products.Add(obj);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product obj)
        {
            using (var contexto = new GeneralPurposeDBEntities())
            {
                //LinQ y Lambda
                var productFound = (from x in contexto.Products
                                    where x.ProductID == obj.ProductID
                                    select x).FirstOrDefault();

                if (productFound != null)
                {
                    productFound.ProductID = obj.ProductID;
                    productFound.Name = obj.Name;
                    productFound.Description = obj.Description;
                    productFound.CategoryID = obj.CategoryID;
                    productFound.UnitPrice = obj.UnitPrice;
                    productFound.ImagePath = obj.ImagePath;
                    productFound.ThumbnailPath = obj.ThumbnailPath;
                    productFound.percentageOff = obj.percentageOff;
                    productFound.SaleStarts = obj.SaleStarts;
                    productFound.SaleEnds = obj.SaleEnds;
                    contexto.SaveChanges();
                }
            }
        }
    }
}