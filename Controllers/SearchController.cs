using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using OnlineShopping.DAL;
using SugarMonkey.Filters;
using SugarMonkey.Repository;

namespace SugarMonkey.Controllers
{
    [FrontPageActionFilter]
    public class SearchController : Controller
    {
        /// <summary>
        ///     Search Result Page
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public ActionResult Index(string searchKey = "")
        {
            ViewBag.searchKey = searchKey;
            List<USP_Search_Result> sr = UnitOfWork.GetRepositoryInstance<USP_Search_Result>()
                .GetResultBySqlProcedure("USP_Search @searchKey",
                    new SqlParameter("searchKey", SqlDbType.VarChar) {Value = searchKey}).ToList();
            return View(sr);
        }

        /// <summary>
        ///     Product Detail
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public ActionResult ProductDetail(int pId)
        {
            Tbl_Product pd = UnitOfWork.GetRepositoryInstance<Tbl_Product>().GetFirstOrDefault(pId);
            ViewBag.SimilarProducts = UnitOfWork.GetRepositoryInstance<Tbl_Product>()
                .GetListByParameter(i => i.CategoryId == pd.CategoryId).ToList();
            return View(pd);
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