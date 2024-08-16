using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaslnyLib.Entity;
using WaslnyLib.Repository;
using WebMatrix.WebData;

namespace WaslnyUi.Controllers
{
    public class CarMaintenancesController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public CarMaintenancesController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }


        // GET: CarMaintenances
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var carMaintenances = db.CarMaintenances.Include(c => c.Car);
            return View(carMaintenances.ToList());
        }

        // GET: CarMaintenances/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMaintenance carMaintenance = db.CarMaintenances.Find(id);
            if (carMaintenance == null)
            {
                return HttpNotFound();
            }
            return View(carMaintenance);
        }

        // GET: CarMaintenances/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN");
            return View();
        }

        // POST: CarMaintenances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarMaintenance model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", model.CarID);
                        _db.CreateCarMaintenance(model.CarMaintenanceDate, model.CarMaintenanceDetails, model.IsDeleted, model.CarID);
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                throw ;
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", model.CarID);
            return View(model);
        }


        // GET: CarMaintenances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMaintenance carMaintenance = db.CarMaintenances.Find(id);
            if (carMaintenance == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", carMaintenance.CarID);
            return View(carMaintenance);
        }

        // POST: CarMaintenances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarMaintenance model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.EditCarMaintenance(model.CarMaintenanceDate, model.CarMaintenanceDetails, model.IsDeleted, model.CarID,model.CarMaintenanceID);
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", model.CarID);
            return View(model);
        }

        // GET: CarMaintenances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarMaintenance carMaintenance = db.CarMaintenances.Find(id);
            if (carMaintenance == null)
            {
                return HttpNotFound();
            }
            return View(carMaintenance);
        }

        // POST: CarMaintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(CarMaintenance model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteCarMaintenance(id);
                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {
                throw ;
            }
            return View(model);
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
