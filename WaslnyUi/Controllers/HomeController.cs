using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace WaslnyUi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!WebSecurity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
            }
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Tables()
        {

            return View();
        }
    }
}