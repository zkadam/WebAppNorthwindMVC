using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppEka.Models;
using WebAppEka.ViewModels;

namespace WebAppEka.Controllers
{
    public class ProdsuppController : Controller
    {
        private northwindEntities db = new northwindEntities();

        // GET: Prodsupp
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Categories).Include(p => p.Suppliers);
            return View(products.ToList());
        }

        // GET: Prodsupp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Prodsupp/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Prodsupp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,ImageLink")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // GET: Prodsupp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // POST: Prodsupp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,ImageLink")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // GET: Prodsupp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Prodsupp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ProductsAndSuppliers()
        {
            var productsSupplier = from p in db.Products
                               join s in db.Suppliers on p.SupplierID equals s.SupplierID
                               
                               
                      //create view for this action remember viewmodels
                      //add link to it from index
                      //debug



                               select new ProdsUpp
                               {

                                   Address = (string)s.Address,
                                   CategoryID = (int)p.CategoryID,
                                   City = (string)s.City,
                                   CompanyName = (string)s.CompanyName,
                                   ContactName = (string)s.ContactName,
                                   ContactTitle = (string)s.ContactTitle,
                                   Country = (string)s.Country,
                                   Discontinued = (bool)p.Discontinued,
                                   Fax = (string)s.Fax,
                                   HomePage = (string)s.HomePage,
                                   ImageLink = (string)p.ImageLink,
                                   Phone = (string)s.Phone,
                                   PostalCode = (string)s.PostalCode,
                                   ProductID = (int)p.ProductID,
                                   ProductName = (string)p.ProductName,
                                   QuantityPerUnit = (string)p.QuantityPerUnit,
                                   Region = (string)s.Region,
                                   ReorderLevel = (int)p.ReorderLevel,
                                   SupplierID = (int)p.SupplierID,
                                   UnitPrice = (float)p.UnitPrice,
                                   UnitsInStock = (int)p.UnitsInStock,
                                   UnitsOnOrder = (int)p.UnitsOnOrder,

                               };
            return View(productsSupplier);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
