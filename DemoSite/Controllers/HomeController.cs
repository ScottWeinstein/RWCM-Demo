namespace DemoSite.Controllers
{
    using System;
    using System.Web.Mvc;
    using DemoSite.Models;
    
    public class HomeController : Controller
    {
        private readonly Func<ProductEF> _prodFactory;
        public HomeController(Func<ProductEF> prodFactory)
        {
            _prodFactory = prodFactory;
            
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Accounts()
        {
            return View(_prodFactory().Accounts);
        }
    }
}
