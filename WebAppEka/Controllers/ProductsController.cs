using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppEka.Models;     //it looks from models

namespace WebAppEka.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1)
        {
            //--------------------------------------------------------login checking if no login, sending the action and controller name so later can return here
            if (Session["UserName"] == null)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("login", "home", new {actionName=actionName, controllerName=controllerName });
            }
            else
            {
                ViewBag.CurrentSort = sortOrder;

                //tää seuraava kaks vaihe vaihtaa viewbagia elli vain sitä että onko seuraavalla klikkaamaalla ascending tai descending
                ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
                ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "UnitPrice_desc" : "UnitPrice";

                northwindEntities db = new northwindEntities();
                var tuotteet = from p in db.Products
                               select p;
                if (!String.IsNullOrEmpty(searchString1))
                {
                    tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1));
                }
                //täälä sitten toteutuu filterointi
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.OrderByDescending(p => p.ProductName);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.OrderBy(p => p.UnitPrice);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.OrderByDescending(p => p.UnitPrice);
                        break;
                    default:
                        tuotteet = tuotteet.OrderBy(p => p.ProductName);
                        break;
                }


                //needs a using sentence
                //List<Products> tuotteet = db.Products.ToList();
                //**************************dbDispose had to be taken coz of new filterings
                //db.Dispose();
                return View(tuotteet);
            }
        }
        
        public ActionResult ProdCards()
        {
            northwindEntities db = new northwindEntities();     //needs a using sentence
            List<Products> tuotteet =  db.Products.ToList();
            db.Dispose();
            return View(tuotteet);
        }

        public ActionResult ProdCards2()
        {
            northwindEntities db = new northwindEntities();     //needs a using sentence
            List<Products> tuotteet = db.Products.ToList();
            db.Dispose();
            return View(tuotteet);
        }
    }
}