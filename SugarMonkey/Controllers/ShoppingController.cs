using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using SugarMonkey.DAL;
using SugarMonkey.Models;
using SugarMonkey.Models.Old.Filters;
using SugarMonkey.Models.Old.Repository;
using SugarMonkey.Models.Views;

namespace SugarMonkey.Controllers
{
    [FrontPageActionFilter]
    [AuthorizeUser]
    public class ShoppingController : Controller
    {
        /// <summary>
        ///     Add Product To Cart
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult AddProductToCart(int productId)
        {
            Tbl_Cart c = new Tbl_Cart
            {
                AddedOn = DateTime.Now,
                CartStatusId = 1,
                MemberId = MemberId,
                ProductId = productId,
                UpdatedOn = DateTime.Now
            };
            UnitOfWork.GetRepositoryInstance<Tbl_Cart>().Add(c);
            UnitOfWork.SaveChanges();
            TempData["ProductAddedToCart"] = "Product added to cart successfully";
            return RedirectToAction("Index", "Search");
        }

        /// <summary>
        ///     MyCart
        /// </summary>
        /// <returns>List of cart items</returns>
        public ActionResult MyCart()
        {
            List<USP_MemberShoppingCartDetails_Result> cd = UnitOfWork
                .GetRepositoryInstance<USP_MemberShoppingCartDetails_Result>().GetResultBySqlProcedure(
                    "USP_MemberShoppingCartDetails @memberId",
                    new SqlParameter("memberId", SqlDbType.Int) {Value = MemberId}).ToList();
            return View(cd);
        }

        /// <summary>
        ///     Remove Cart Item
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ActionResult RemoveCartItem(int productId)
        {
            Tbl_Cart c = UnitOfWork.GetRepositoryInstance<Tbl_Cart>().GetFirstOrDefaultByParameter(i =>
                i.ProductId == productId && i.MemberId == MemberId && i.CartStatusId == 1);
            c.CartStatusId = 2;
            c.UpdatedOn = DateTime.Now;
            UnitOfWork.GetRepositoryInstance<Tbl_Cart>().Update(c);
            UnitOfWork.SaveChanges();
            return RedirectToAction("MyCart");
        }

        /// <summary>
        ///     CheckOut the Cart items
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckOut()
        {
            List<USP_MemberShoppingCartDetails_Result> cd = UnitOfWork
                .GetRepositoryInstance<USP_MemberShoppingCartDetails_Result>().GetResultBySqlProcedure(
                    "USP_MemberShoppingCartDetails @memberId",
                    new SqlParameter("memberId", SqlDbType.Int) {Value = MemberId}).ToList();
            ViewBag.TotalPrice = cd.Sum(i => i.Price);
            ViewBag.CartIds = string.Join(",", cd.Select(i => i.CartId).ToList());
            return View(cd);
        }

        /// <summary>
        ///     Payment Success
        /// </summary>
        /// <param name="shippingDetails"></param>
        /// <returns></returns>
        public ActionResult PaymentSuccess(ShippingDetails shippingDetails)
        {
            Tbl_ShippingDetails sd = new Tbl_ShippingDetails
            {
                MemberId = MemberId,
                AddressLine = shippingDetails.Address,
                City = shippingDetails.City,
                State = shippingDetails.State,
                Country = shippingDetails.Country,
                ZipCode = shippingDetails.ZipCode,
                OrderId = Guid.NewGuid().ToString(),
                AmountPaid = shippingDetails.TotalPrice,
                PaymentType = shippingDetails.PaymentType
            };
            UnitOfWork.GetRepositoryInstance<Tbl_ShippingDetails>().Add(sd);
            UnitOfWork.GetRepositoryInstance<Tbl_Cart>()
                .UpdateByWhereClause(i => i.MemberId == MemberId && i.CartStatusId == 1, j => j.CartStatusId = 3);
            UnitOfWork.SaveChanges();
            if (!string.IsNullOrEmpty(Request["CartIds"]))
            {
                int[] cartIdsToUpdate = Request["CartIds"].Split(',').Select(int.Parse).ToArray();
                UnitOfWork.GetRepositoryInstance<Tbl_Cart>().UpdateByWhereClause(
                    i => cartIdsToUpdate.Contains(i.CartId), j => j.ShippingDetailId = sd.ShippingDetailId);
                UnitOfWork.SaveChanges();
            }

            return View(sd);
        }

        #region Other Class references ...

        // Instance on Unit of Work         
        public GenericUnitOfWork UnitOfWork = new GenericUnitOfWork();
        private int _memberId;

        public int MemberId
        {
            get => Convert.ToInt32(Session["MemberId"]);
            set => _memberId = Convert.ToInt32(Session["MemberId"]);
        }

        #endregion
    }
}