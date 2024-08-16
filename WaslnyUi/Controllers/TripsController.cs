using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaslnyLib.Entity;
using WaslnyLib.Repository;
using WebMatrix.WebData;

namespace WaslnyUi.Controllers
{
    public class TripsController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        private DateTime restTime = DateTime.Now.AddHours(-12);

        public TripsController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }


        // GET: Trips
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var trips = db.Trips.Where(c => c.TripStatus == "UpComing").Include(t => t.Car).Include(t => t.Customer).Include(t => t.Driver).Include(t => t.Route);
            return View(trips.ToList());
        }
        public ActionResult Index2()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var trips = db.Trips.Where(c=>c.TripStatus== "Pending").Include(t => t.Car).Include(t => t.Customer).Include(t => t.Driver).Include(t => t.Route);
            return View(trips.ToList());
        }
        public ActionResult Index3()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var trips = db.Trips.Where(c => c.TripStatus == "Completed").Include(t => t.Car).Include(t => t.Customer).Include(t => t.Driver).Include(t => t.Route);
            return View(trips.ToList());
        }

        // GET: Trips/Details/5
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
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // GET: Trips/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN");
            ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.IsDeleted), "CustomerID", "CustomerPhone");
            ViewBag.DriverID = new SelectList(db.Drivers.Where(d => !d.IsDeleted && (d.DriverLastTripDT == null || d.DriverLastTripDT < restTime)), "DriverID", "DriverPhone");
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Trip model)
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
                _db.CreateTrip(model.TripBookingDT, model.TripPredictedTravelDT, model.TripPassengers, model.TripStatus, model.IsDeleted, model.DriverID, model.CarID, model.RouteID, model.CustomerID);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Driver's License has expired"))
                {
                    ModelState.AddModelError("DriverLED", "The Driver's License has expired.");
                }
                else if (ex.Message.Contains("Car's license has expired"))
                {
                    ModelState.AddModelError("CarLED", "The Car's license has expired.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", model.CarID);
            ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.IsDeleted), "CustomerID", "CustomerPhone", model.CustomerID);
            ViewBag.DriverID = new SelectList(db.Drivers.Where(d => !d.IsDeleted && (d.DriverLastTripDT == null || d.DriverLastTripDT < restTime)), "DriverID", "DriverPhone", model.DriverID);
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo", model.RouteID);
            return View(model);
        }
        // GET: Trips/Edit/5
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
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", trip.CarID);
            ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.IsDeleted), "CustomerID", "CustomerPhone", trip.CustomerID);
            ViewBag.DriverID = new SelectList(db.Drivers.Where(d => !d.IsDeleted && (d.DriverLastTripDT == null || d.DriverLastTripDT < restTime)), "DriverID", "DriverPhone", trip.DriverID);
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo", trip.RouteID);
            return View(trip);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Trip model)
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
                _db.EditTrip(model.TripBookingDT, model.TripRealTravelDT, model.TripPredictedTravelDT, model.TripArrivalDT, model.TripStartKM, model.TripEndKM, model.TripPassengers, model.TripStatus, model.TripDriverRating, model.TripCarRating, model.TripComments, model.IsDeleted, model.DriverID, model.CarID, model.RouteID, model.CustomerID, model.TripID);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Driver's License has expired"))
                {
                    ModelState.AddModelError("DriverLED", "The Driver's License has expired.");
                }
                else if (ex.Message.Contains("Car's license has expired"))
                {
                    ModelState.AddModelError("CarLED", "The Car's license has expired.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            ViewBag.CarID = new SelectList(db.Cars.Where(c => !c.IsDeleted), "CarID", "CarPN", model.CarID);
            ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.IsDeleted), "CustomerID", "CustomerPhone", model.CustomerID);
            ViewBag.DriverID = new SelectList(db.Drivers.Where(d => !d.IsDeleted && (d.DriverLastTripDT == null || d.DriverLastTripDT < restTime)), "DriverID", "DriverPhone", model.DriverID);
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo", model.RouteID);
            return View(model);
        }


        // GET: Trips/Delete/5
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
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Trip model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteTrip(id);
                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return View(model);
        }

        // GET: Trips/StartTrip/5
        public ActionResult StartTrip(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                _db.StartTrip(id.Value);

                return RedirectToAction("Index2");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Trip not found"))
                {
                    ModelState.AddModelError("", "The specified trip does not exist.");
                }
                else if (ex.Message.Contains("Car not found"))
                {
                    ModelState.AddModelError("", "The specified car does not exist.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return RedirectToAction("Edit", new { id });
        }
        public ActionResult EndTrip(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                _db.EndTrip(id.Value);

                return RedirectToAction("Index3");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Trip not found"))
                {
                    ModelState.AddModelError("", "The specified trip does not exist.");
                }
                else if (ex.Message.Contains("Car not found"))
                {
                    ModelState.AddModelError("", "The specified car does not exist.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return RedirectToAction("Edit", new { id });
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
