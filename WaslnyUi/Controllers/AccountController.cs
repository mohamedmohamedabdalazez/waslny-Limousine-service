using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WaslnyLib.Entity;
using WaslnyLib.Repository;
using WaslnySecurity.Entity;
using WaslnySecurity.Repository;
using WebMatrix.WebData;
using static WaslnyUi.Models.AccountModel;

namespace WaslnyUi.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWaslnySecurityRepository _db;
        public AccountController()
        {
            _db = new WaslnySecurityRepository();
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("MembershipDbContext", "Users", "UserId", "UserName", autoCreateTables: true);
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: false))
            {   
                Session["login"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            
            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("MembershipDbContext", "Users", "UserId", "UserName", autoCreateTables: true);
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new WaslnySecurityDbContext())
                {
                    if (db.Users.Any(u => u.UserName == model.UserName))
                    {
                        ModelState.AddModelError("", "UserName already exists. Please choose a different UserName.");
                        return View(model);
                    }
                    try
                    {
                        WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }
            }

            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            Session["login"] = null;
            return RedirectToAction("Login", "Account");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";
                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";
                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";
                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";
                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again.";
                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again.";
                default:
                    return "An unknown error occurred. Please verify your entry and try again.";
            }
        }
    }
}