using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LunchTrainWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
            {
                ViewBag.DisplayName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            return View();
        }
    }
}
