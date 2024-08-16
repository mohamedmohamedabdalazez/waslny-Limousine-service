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
    public class RoutesController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public RoutesController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }


        // GET: Routes
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Routes.ToList());
        }

        // GET: Routes/Details/5
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
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Route model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.CreateRoute(model.RouteTo,model.RouteFrom, model.IsDeleted);
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return View(model);
        }
        // GET: Routes/Edit/5
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
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Route model)
        {
            try
            {
                if (Session["login"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        _db.EditRoute(model.RouteTo, model.RouteFrom, model.IsDeleted,model.RouteID);
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return View(model);
        }

        // GET: Routes/Delete/5
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
            Route route = db.Routes.Find(id);
            if (route == null)
            {
                return HttpNotFound();
            }
            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Route model, int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteRoute(id);
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
