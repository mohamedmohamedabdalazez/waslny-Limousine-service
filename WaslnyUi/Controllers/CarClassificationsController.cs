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
    public class CarClassificationsController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public CarClassificationsController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }

        // GET: CarClassifications
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var carClassifications = db.CarClassifications.Include(c => c.Car);
            return View(carClassifications.ToList());
        }

        // GET: CarClassifications/Details/5
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
            CarClassification carClassification = db.CarClassifications.Find(id);
            if (carClassification == null)
            {
                return HttpNotFound();
            }
            return View(carClassification);
        }

        // GET: CarClassifications/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN");
            return View();
        }

        // POST: CarClassifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarClassification model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.CreateCarClassification(model.CarClassificationQuality, model.CarClassificationType, model.IsDeleted,model.CarID);
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
       
        // GET: CarClassifications/Edit/5
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
            CarClassification carClassification = db.CarClassifications.Find(id);
            if (carClassification == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", carClassification.CarID);
            return View(carClassification);
        }

        // POST: CarClassifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarClassification model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.EditCarClassification(model.CarClassificationQuality, model.CarClassificationType, model.IsDeleted, model.CarID,model.CarClassificationID);
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

        // GET: CarClassifications/Delete/5
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
            CarClassification carClassification = db.CarClassifications.Find(id);
            if (carClassification == null)
            {
                return HttpNotFound();
            }
            return View(carClassification);
        }

        // POST: CarClassifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(CarClassification model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteCarClassification(id);
                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {
                throw;
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
