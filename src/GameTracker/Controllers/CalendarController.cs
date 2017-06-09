using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameTracker.Controllers
{
    public class CalendarController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(DateTime? sentDate = null)
        {
            //according to Stackoverflow, default(DateTime) causing an exception here is a bug

            if (sentDate == null)
                sentDate = DateTime.Today;

            return View(sentDate);
        }
    }
}
