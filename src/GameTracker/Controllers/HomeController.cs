using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using GameTracker.Models;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using GameTracker.Data;

namespace GameTracker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("Calendar");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
