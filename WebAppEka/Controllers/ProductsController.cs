using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppEka.Models;     //it looks from models
using WebAppEka.ViewModels;
using PagedList;
using System.Data.Entity.SqlServer;

namespace WebAppEka.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1, string ProductCategory, string currentProductCategory, int? page, int? pagesize)
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

//tää seuraava kaks vaihe vaihtaa viewbagia eli vain sitä että onko seuraavalla klikkaamaalla ascending tai descending
                ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
                ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "UnitPrice_desc" : "UnitPrice";

// jos laitettiin joku searchiin, mene 1.sivuun

//hakufiltterin muistiin
                if (searchString1!=null)
                {
                    page = 1;
                }
 //muuten annetaan searchstringille filterin arvo - koska filter jää muistossa - sitä aina lehetätään viewin kautta(alhalla oleva acition url)
                else
                {
                    searchString1 = currentFilter1;
                }
                ViewBag.currentFilter1 = searchString1;

//tuottekategoriahakufiltterin laitto muistiin

                if ((ProductCategory !=null) && (ProductCategory !="0"))
                {
                    page = 1;
                }
                else
                {
                    ProductCategory = currentProductCategory;
                }
                ViewBag.currentProductCategory = ProductCategory;




        northwindEntities db = new northwindEntities();
        var tuotteet = from p in db.Products
                        select p;

//filtering ONLY by product category 
                if (!String.IsNullOrEmpty(ProductCategory) && (ProductCategory != "0"))
                {
                    int para = int.Parse(ProductCategory);
                    tuotteet = tuotteet.Where(p => p.CategoryID == para);
                }
//Filtering by search                }
         //jos hakufiltteri on käytössä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
                if (!String.IsNullOrEmpty(searchString1))  
                {

    //ordering the search results      -- category has been filtered already             
                    switch (sortOrder)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderByDescending(p => p.ProductName);
                            break;
                        case "UnitPrice":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderBy(p => p.UnitPrice);
                            break;
                        case "UnitPrice_desc":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderByDescending(p => p.UnitPrice);
                            break;
                        default:
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderBy(p => p.ProductName);
                            break;
                    }
                }
                //here ordering if there is no searchword but there is category choosen
                else if (!String.IsNullOrEmpty(ProductCategory) && (ProductCategory != "0"))
                {
                    int para = int.Parse(ProductCategory);

                    switch (sortOrder)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderByDescending(p => p.ProductName);
                            break;
                        case "UnitPrice":
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderBy(p => p.UnitPrice);
                            break;
                        case "UnitPrice_desc":
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderByDescending(p => p.UnitPrice);
                            break;
                        default:
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderBy(p => p.ProductName);
                            break;
                    }
                }
                //täälä sitten toteutuu järjestely ilman filtteri
                else
                {

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
                }


                                //needs a using sentence
                                //List<Products> tuotteet = db.Products.ToList();
                                //**************************dbDispose had to be taken coz of new filterings
                                //db.Dispose();


 //creating list for Categories dropdown
                List<Categories> lstCategories = new List<Categories>();
//bringing categories to apumuuttuja
                var categoryList = from cat in db.Categories    
                                   select cat;
                Categories tyhjaCategory = new Categories();    //creating an empty category which is needed, if no category has been selected
                tyhjaCategory.CategoryID = 0;
                tyhjaCategory.CategoryName = "";
                tyhjaCategory.CategoryIdCategoryName = "";  //HUOM pitää lisätä MODELS kansion luokkamääräyksen public string CategoryIDCategoryName{get;set}
                lstCategories.Add(tyhjaCategory);

                //bringing the categories to the list
                foreach (Categories category in categoryList)
                {
                    Categories yksiCategory = new Categories();
                    yksiCategory.CategoryID = category.CategoryID;
                    yksiCategory.CategoryName = category.CategoryName;
                    yksiCategory.CategoryIdCategoryName = category.CategoryID.ToString() + " - " + category.CategoryName;  //HUOM pitää lisätä MODELS kansion luokkamääräyksen public string CategoryIDCategoryName{get;set}
                    lstCategories.Add(yksiCategory);
                }
                ViewBag.CategoryID = new SelectList(lstCategories, "CategoryID", "CategoryIDCategoryName", ProductCategory); //lopuks luodaan SelectLitin ja sijoitetaan sen Viewbagiin

               

                int pageSize = (pagesize ?? 10); //tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); //Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron 1
                return View(tuotteet.ToPagedList(pageNumber, pageSize));
            }
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         //------------------------------------------------------------------------prodcards actions -------------------------------------------------------------------------------------       
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
        public ActionResult _ProductSalesPerDate(string productName)
        {
            //just to show something if productname is null, otherwise maybe it is better to use id as parameter
            if (String.IsNullOrEmpty(productName)) { productName = "Lakkalikööri"; }

            List<DailyProductSales> dailyProductSalesList = new List<DailyProductSales>();
            northwindEntities db = new northwindEntities();     //needs a using sentence

            var orderSummary = from pds in db.ProductsDailySales
                               where pds.ProductName == productName
                               orderby pds.OrderDate
                               select new DailyProductSales
                               {
                                   OrderDate = SqlFunctions.DateName("year", pds.OrderDate) + "." + SqlFunctions.DateName("MM", pds.OrderDate) + "." + SqlFunctions.DateName("dar", pds.OrderDate),
                                   DailySales = (float)pds.DailySales,
                                   ProductName = pds.ProductName
                               };


           
            return Json(orderSummary, JsonRequestBehavior.AllowGet);
        }
    }
}