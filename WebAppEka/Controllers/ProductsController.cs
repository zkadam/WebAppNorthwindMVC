﻿using System;
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
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("login", "home", new {actionName=actionName, controllerName=controllerName });
            }
            else
            {
                northwindEntities db = new northwindEntities();
                var tuotteet = from p in db.Products
                               select p;
                //if (!String.IsNullOrEmpty(searchString1))
                //{
                //    tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1));
                //}
                
                
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