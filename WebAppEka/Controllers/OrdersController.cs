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
using PagedList;

namespace WebAppEka.Controllers
{
    public class OrdersController : Controller
    {
        private northwindEntities db = new northwindEntities();

        // GET: Orders
        public ActionResult Index(string currentFilter1, string OrderShipper, string currentOrderShipper, int? page, int? pagesize)
        {
            //  var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers);
            northwindEntities db = new northwindEntities();
            var orders = from o in db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers).OrderBy(o =>o.OrderDate)
                         select o;

            //tuottekategoriahakufiltterin laitto muistiin

            if ((OrderShipper != null) && (OrderShipper != "0"))
            {
                page = 1;
            }
            else
            {
                OrderShipper = currentOrderShipper;
            }
            ViewBag.currentOrderShipper = OrderShipper;

// filtering ONLY by shipper companies
                if (!String.IsNullOrEmpty(OrderShipper) && (OrderShipper != "0"))
                {
                    int para = int.Parse(OrderShipper);
                    orders = orders.Where(o => o.ShipVia == para);
                }








            //creating list for Categories dropdown
            List<Shippers> lstShippers = new List<Shippers>();
//bringing shippers to apumuuttuja
            var shippersList = from ship in db.Shippers
                               select ship;
           Shippers tyhjaShipper = new Shippers();    //creating an empty category which is needed, if no category has been selected
            tyhjaShipper.ShipperID = 0;
            tyhjaShipper.CompanyName = "";
            lstShippers.Add(tyhjaShipper);

            //bringing the shippers to the list
            foreach (Shippers shipper in shippersList)
            {
                Shippers yksiShipper = new Shippers();
                yksiShipper.ShipperID = shipper.ShipperID;
                yksiShipper.CompanyName = shipper.CompanyName;
                lstShippers.Add(yksiShipper);
            }
            ViewBag.ShipperId = new SelectList(lstShippers, "ShipperID", "CompanyName", OrderShipper); //lopuks luodaan SelectLitin ja sijoitetaan sen Viewbagiin




            //the paged nutget extention needs a pagenumber int and a pagesize int which we send from view but if it would be null, we change it here because you cant have 0th page
            int pageSize = (pagesize ?? 10);
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
            //return View(orders);
        }

       
        
        
        
        
        
        
        
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }
        
        
        
        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }
        //-----------------------------------------------------------------------------------------Modal edit-----------------------------------------------------------------------------
        // GET: Orders/Edit/5
        public ActionResult _ModalEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return PartialView("_ModalEdit", orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _ModalEdit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return PartialView("_ModalEdit",orders);
        }
        //-------------------------------------------------------------------------------------------------delete-------------------------------------------------------------------------------
        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //--------------------------------------------------------------------------------------------------viewmodels ordersummary--------------------------------------------------------------------------------------//

        public ActionResult Ordersummary()
        {
            var orderSummary = from o in db.Orders
                               join od in db.Order_Details on o.OrderID equals od.OrderID
                               join p in db.Products on od.ProductID equals p.ProductID
                               join c in db.Categories on p.CategoryID equals c.CategoryID
                               //where lause
                               //orderby lause



                               select new OrderSummaryData
                               {

                                   OrderID = (int)o.OrderID,
                                   CustomerID = (string)o.CustomerID,
                                   EmployeeID = (int)o.EmployeeID,
                                   OrderDate = (DateTime)o.OrderDate,
                                   RequiredDate = (DateTime)o.RequiredDate,
                                   ShippedDate = (DateTime)o.ShippedDate,
                                   ShipVia = (int)o.ShipVia,
                                   Freight = (float)o.Freight,
                                   ShipName = (string)o.ShipName,
                                   ShipAddress = (string)o.ShipAddress,
                                   ShipCity = (string)o.ShipCity,
                                   ShipRegion = (string)o.ShipRegion,
                                   ShipPostalCode = (string)o.ShipPostalCode,
                                   ShipCountry = (string)o.ShipCountry,
                                   ProductID = (int)p.ProductID,
                                   UnitPrice = (float)p.UnitPrice,
                                   Quantity = (int)od.Quantity,
                                   Discount = (float)od.Discount,
                                   ProductName = (string)p.ProductName,
                                   SupplierID = (int)p.SupplierID,
                                   CategoryID = (int)c.CategoryID,
                                   QuantityPerUnit = (string)p.QuantityPerUnit,
                                   UnitsInStock = (int)p.UnitsInStock,
                                   UnitsOnOrder = (int)p.UnitsOnOrder,
                                   ReorderLevel = (int)p.ReorderLevel,
                                   Discontinued = (bool)p.Discontinued,
                                   ImageLink = (string)p.ImageLink,
                                   CategoryName = (string)c.CategoryName,
                                   Description = (string)c.Description,
                                   // Picture = (Image)p.Picture,
                               };

            return View(orderSummary);
        }

        //--------------------------------------------------------------------------------------------------order headers--------------------------------------------------------------------------------------//

        public ActionResult TilausOtsikot(string searchAsiakas, string searchKaupunki, string searchRahtari, string currentTextfield, int? page, int? pagesize)
        {
       
            // jos laitettiin joku searchiin, mene 1.sivuun

            //hakufiltterin muistiin
            if ((searchAsiakas != null)&& (searchKaupunki != null) && (searchRahtari != null))
            {
                page = 1;
            }
        //if arvo is null, we change it to blank
            searchAsiakas = (searchAsiakas ?? "");
            searchKaupunki = (searchKaupunki ?? "");
            searchRahtari = (searchRahtari ?? "");
           //we set the focus of textbox for the last entery or to asiakas
            currentTextfield = (currentTextfield ?? "searchAsiakas");

            //sending the values to browser, so it fills up the fields of textboxes

            ViewBag.currentAsiakas = searchAsiakas;
            ViewBag.currentKaupunki = searchKaupunki;
            ViewBag.currentRahtari = searchRahtari;


//sending browser where was the last search input
            ViewBag.currentTextfield = currentTextfield;

            //-----------------------------------------------------------filtering with jquery-----------------------------------------------------------------------------------------------------//
            var orders = from ord in db.Orders.Include(ord => ord.Customers).Include(ord => ord.Employees).Include(ord => ord.Shippers)
                         
                         where (ord.Customers.CompanyName.Contains(searchAsiakas) && ord.Customers.City.Contains(searchKaupunki) && ord.Shippers.CompanyName.Contains(searchRahtari))
                         orderby ord.OrderDate
                         select ord;



            int pageSize = (pagesize ?? 10);
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        //---------------------------------------------------------------- tilausotsikot masterView tehtävän varten AZ---------------------------------------------------------------------------------------------------//
        public ActionResult _TilausRivit(int? orderid)
        {




            var orderRowsList = from od in db.Order_Details
                               join p in db.Products on od.ProductID equals p.ProductID
                               join c in db.Categories on p.CategoryID equals c.CategoryID
                               where od.OrderID== orderid
                               //orderby lause



                               select new OrderRows
                              {

                                  OrderID = (int)od.OrderID,
                                  ProductID = (int)p.ProductID,
                                  UnitPrice = (float)p.UnitPrice,
                                  Quantity = (int)od.Quantity,
                                  Discount = (float)od.Discount,
                                  ProductName = (string)p.ProductName,
                                  SupplierID = (int)p.SupplierID,
                                  CategoryID = (int)c.CategoryID,
                                  QuantityPerUnit = (string)p.QuantityPerUnit,
                                  UnitsInStock = (int)p.UnitsInStock,
                                  UnitsOnOrder = (int)p.UnitsOnOrder,
                                  ReorderLevel = (int)p.ReorderLevel,
                                  Discontinued = (bool)p.Discontinued,
                                  ImageLink = (string)p.ImageLink,
                                  CategoryName = (string)c.CategoryName,
                                  Description = (string)c.Description,
                                  // Picture = (Image)p.Picture,
                              };

            return PartialView(orderRowsList);


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
