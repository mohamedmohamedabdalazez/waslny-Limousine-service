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
    public class DriversController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public DriversController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }

        // GET: Drivers
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Drivers.ToList());
        }

        // GET: Drivers/Details/5
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
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // GET: Drivers/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Driver model)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _db.CreateDriver(model.DriverFName, model.DriverLName, model.DriverPhone, model.DriverSSN, model.DriverSalary, model.DriverDOB, model.DriverAddress, model.DriverPhoto, model.DriverCity, model.DriverLED, model.IsDeleted);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Driver Phone Number already exists"))
                {
                    ModelState.AddModelError("DriverPhone", "The Driver Phone Number already exists.");
                }
                else if (ex.Message.Contains("Driver SSN Number already exists"))
                {
                    ModelState.AddModelError("DriverSSN", "The Driver SSN Number already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Drivers/Edit/5
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
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Driver model)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _db.EditDriver(model.DriverFName, model.DriverLName, model.DriverPhone, model.DriverSSN, model.DriverSalary, model.DriverDOB, model.DriverAddress, model.DriverPhoto, model.DriverCity, model.DriverLED, model.DriverLastTripDT, model.IsDeleted, model.DriverID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Driver Phone Number already exists"))
                {
                    ModelState.AddModelError("DriverPhone", "The Driver Phone Number already exists.");
                }
                else if (ex.Message.Contains("Driver SSN Number already exists"))
                {
                    ModelState.AddModelError("DriverSSN", "The Driver SSN Number already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Drivers/Delete/5
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
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Driver model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteDriver(id);
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
