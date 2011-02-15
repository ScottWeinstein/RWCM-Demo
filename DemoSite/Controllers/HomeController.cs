using System;
using System.Web.Mvc;

namespace DemoSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
