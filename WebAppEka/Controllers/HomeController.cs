using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppEka.Models;

namespace WebAppEka.Controllers
{
    public class HomeController : Controller
    {
   
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            northwindEntities db = new northwindEntities();
            //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                Session["UserName"] = LoggedUser.UserName;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoggedStatus = "Out";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Login", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen pääsivulle
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public  ActionResult  Laskuri()
        {
            string apumuuttuja = "";
            for (int i = 0; i < 10; i++)
            {
                apumuuttuja = apumuuttuja + "Laskuri on nyt= " + i.ToString() +" ";
                
            }
            ViewBag.Message = apumuuttuja;
            return View();

        }

        public ActionResult Map()
        {
            return View();
        }

    
    
    
    }


}