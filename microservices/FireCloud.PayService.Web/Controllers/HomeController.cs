﻿using Microsoft.AspNetCore.Mvc;

namespace FireCloud.PayService.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
