using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameTracker.Controllers
{
    public class CalendarController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string id = null)
        {
            //according to Stackoverflow, default(DateTime) causing an exception here is a bug

            DateTime sentDate = new DateTime();

            if (id == null)
                sentDate = DateTime.Today;
            else
                if (DateTime.TryParseExact(id, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out sentDate))
                //setting this to the first would maybe look better, seems to serve no purpose
                return View(sentDate);
                else
                sentDate = DateTime.Today;

            return View(sentDate);
        }
    }
}
