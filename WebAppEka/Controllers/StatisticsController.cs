using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppEka.Models;
using WebAppEka.ViewModels;

namespace WebAppEka.Controllers
{
    public class StatisticsController : Controller
    {
        private northwindEntities db = new northwindEntities();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategorySales()
        {

            string categoryNameList;
            string categorySalesList;
            List<CategorySalesClass> CategorySalesList = new List<CategorySalesClass>();

            var categorySalesData = from cs in db.Category_Sales_for_1997
                                    select cs;
            foreach (Category_Sales_for_1997 salesfor1997 in categorySalesData)
            {
                CategorySalesClass OneSalesRow = new CategorySalesClass();
                OneSalesRow.CategoryName = salesfor1997.CategoryName;//nämä ovat databasein tableviewin kentät
                OneSalesRow.CategorySales = (int)salesfor1997.CategorySales;
                CategorySalesList.Add(OneSalesRow);
            }
            //string joinnilla viedään kaikki list elementtit kaks string riviin x ja y varteen- laitetaan pilkut ja qmarksit väliin
            categoryNameList = "'" + string.Join("','", CategorySalesList.Select(n=>n.CategoryName).ToList())+ "'";
            categorySalesList =string.Join(",", CategorySalesList.Select(n=>n.CategorySales).ToList());

            ViewBag.categoryName = categoryNameList;
            ViewBag.categorySales = categorySalesList;
            return View();
        }

    }
}