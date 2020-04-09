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
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                northwindEntities db = new northwindEntities();     //needs a using sentence
                List<Products> tuotteet = db.Products.ToList();
                db.Dispose();
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