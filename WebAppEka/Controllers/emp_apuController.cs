using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppEka.Models;

namespace WebAppEka.Controllers
{
    public class emp_apuController : Controller
    {
        private northwindEntities db = new northwindEntities();

        // GET: Empaapu
        public ActionResult Index()
        {
            return View(db.emp_apu.ToList());
        }

        // GET: Empaapu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emp_apu emp_apu = db.emp_apu.Find(id);
            if (emp_apu == null)
            {
                return HttpNotFound();
            }
            return View(emp_apu);
        }

        // GET: Empaapu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empaapu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath")] emp_apu emp_apu)
        {
            if (ModelState.IsValid)
            {
                db.emp_apu.Add(emp_apu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emp_apu);
        }
        //*************************************filtered view
        public ActionResult FilteredView(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //***********************************here the id had to be rewritten coz employee id wasnt set as primary key***********************// 

            //List<emp_apu> filtered = emp_apu.Where(m => m.ReportsTo == id).ToList();

            //emp_apu Empaapu = db.emp_apu.Where(m => m.EmployeeID == id);
            // var offerList = this.GetOfferList(id)
            //fferList.Offers = offerList.Offers.Where(o => !o.IsSealed));
            //var filtered = db.emp_apu(id).To.List().Where(m => m.EmployeeID == id));

           // var filteredList = db.emp_apu.ToList();
            var filtered = db.emp_apu.Where(m => m.ReportsTo == id).ToList();


            if (filtered == null)
            {
                return HttpNotFound();
            }
            return View(filtered);
            
        }
        // GET: Empaapu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           //***********************************here the id had to be rewritten coz employee id wasnt set as primary key***********************// 
            emp_apu Empaapu = db.emp_apu.SingleOrDefault(m => m.EmployeeID == id);
            if (Empaapu == null)
            {
                return HttpNotFound();
            }
            return View(Empaapu);
        }


        // POST: Empaapu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath")] emp_apu emp_apu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp_apu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp_apu);
        }

        // GET: Empaapu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emp_apu emp_apu = db.emp_apu.Find(id);
            if (emp_apu == null)
            {
                return HttpNotFound();
            }
            return View(emp_apu);
        }

        // POST: Empaapu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            emp_apu emp_apu = db.emp_apu.Find(id);
            db.emp_apu.Remove(emp_apu);
            db.SaveChanges();
            return RedirectToAction("Index");
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
