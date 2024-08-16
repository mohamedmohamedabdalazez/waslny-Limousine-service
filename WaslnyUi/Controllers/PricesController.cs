using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WaslnyLib.Entity;
using WaslnyLib.Repository;
using WebMatrix.WebData;

namespace WaslnyUi.Controllers
{
    public class PricesController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public PricesController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }

        // GET: Prices
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var prices = db.Prices.Include(p => p.CarClassification).Include(p => p.Route);
            return View(prices.ToList());
        }

        // GET: Prices/Details/5
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
            Price price = db.Prices.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        // GET: Prices/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.CarClassificationID = new SelectList(db.CarClassifications.Where(c => !c.IsDeleted), "CarClassificationID", "CarClassificationType");
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo");
            return View();
        }

        // POST: Prices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Price model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.CreatePrice(model.PriceMoney, model.IsDeleted,model.RouteID,model.CarClassificationID);
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            ViewBag.CarClassificationID = new SelectList(db.CarClassifications.Where(c => !c.IsDeleted), "CarClassificationID", "CarClassificationType", model.CarClassificationID);
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo", model.RouteID);
            return View(model);
        }
        // GET: Prices/Edit/5
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
            Price price = db.Prices.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarClassificationID = new SelectList(db.CarClassifications.Where(c => !c.IsDeleted), "CarClassificationID", "CarClassificationType", price.CarClassificationID);
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo", price.RouteID);
            return View(price);
        }

        // POST: Prices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Price model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.EditPrice(model.PriceMoney, model.IsDeleted, model.RouteID, model.CarClassificationID,model.PriceID);
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            ViewBag.CarClassificationID = new SelectList(db.CarClassifications.Where(c => !c.IsDeleted), "CarClassificationID", "CarClassificationType", model.CarClassificationID);
            ViewBag.RouteID = new SelectList(db.Routes.Where(r => !r.IsDeleted), "RouteID", "RouteTo", model.RouteID);
            return View(model);
        }

        // GET: Prices/Delete/5
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
            Price price = db.Prices.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Price model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeletePrice(id);
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
