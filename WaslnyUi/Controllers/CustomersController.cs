using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.Mvc;
using WaslnyLib.Entity;
using WaslnyLib.Repository;
using WebMatrix.WebData;

namespace WaslnyUi.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IWaslnyLibRepository _db;
        private readonly int loggedUserId;
        private WaslnyLibDbContext db = new WaslnyLibDbContext();
        public CustomersController()
        {
            _db = new WaslnyLibRepository();
            loggedUserId = WebSecurity.CurrentUserId;
        }
        // GET: Customers
        public ActionResult Index()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
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
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer model)
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
                _db.CreateCustomer(model.CustomerFName, model.CustomerLName, model.CustomerGender, model.CustomerAddress, model.CustomerPhone, model.CustomerCity, model.CustomerEmail, model.IsDeleted, model.CustomerDOB);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Customer Phone Number already exists"))
                {
                    ModelState.AddModelError("CustomerPhone", "The Customer Phone Number already exists.");
                }
                else if (ex.Message.Contains("Customer Email already exists"))
                {
                    ModelState.AddModelError("CustomerEmail", "The Customer Email already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Customers/Edit/5
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
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer model)
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
                _db.EditCustomer(model.CustomerFName, model.CustomerLName, model.CustomerGender, model.CustomerAddress, model.CustomerPhone, model.CustomerCity, model.CustomerEmail, model.IsDeleted, model.CustomerID, model.CustomerDOB);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Customer Phone Number already exists"))
                {
                    ModelState.AddModelError("CustomerPhone", "The Customer Phone Number already exists.");
                }
                else if (ex.Message.Contains("Customer Email already exists"))
                {
                    ModelState.AddModelError("CustomerEmail", "The Customer Email already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Customers/Delete/5
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
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Customer model , int id)
        {
            try
            {
                if (Session["login"] != null)
                {

                    _db.DeleteCustomer(id);
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
