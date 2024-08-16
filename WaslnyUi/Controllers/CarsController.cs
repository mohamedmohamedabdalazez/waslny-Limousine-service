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
    public class CarsController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public CarsController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }

        // GET: Cars
        [Authorize]
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Cars.ToList());
        }

        // GET: Cars/Details/5
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
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car model)
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
                _db.CreateCar(model.CarColor, model.CarBrand, model.CarModel, model.CarVersion, model.CarType, model.CarCapacity, model.CarPN, model.CarLED, model.CarRented, model.CarKMDashboard, model.CarPhoto, model.CarIsAvailable, model.IsDeleted);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("plate number already exists"))
                {
                    ModelState.AddModelError("CarPN", "The car plate number already exists.");
                }
                else if (ex.Message.Contains("license has expired"))
                {
                    ModelState.AddModelError("CarLED", "The car's license has expired.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(model);
        }


        // GET: Cars/Edit/5
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
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car model)
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
                _db.EditCar(model.CarColor, model.CarBrand, model.CarModel, model.CarVersion, model.CarType, model.CarCapacity, model.CarPN, model.CarLED, model.CarRented, model.CarKMDashboard, model.CarPhoto, model.CarIsAvailable, model.IsDeleted, model.CarID);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("plate number already exists"))
                {
                    ModelState.AddModelError("CarPN", "The car plate number already exists.");
                }
                else if (ex.Message.Contains("license has expired"))
                {
                    ModelState.AddModelError("CarLED", "The car's license has expired.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Cars/Delete/5
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
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Car model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteCar(id);
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
