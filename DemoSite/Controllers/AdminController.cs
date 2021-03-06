﻿namespace DemoSite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using DemoSite.Models;
    using DemoSite.Services;

    public class AdminController : Controller
    {
        private readonly DemoConfig _config;

        public AdminController(DemoConfig config)
        {
            _config = config;
        }

        public ActionResult Diag()
        {
            return View(EnvEvaluator.Evaluate(_config));
        }
    }
}
