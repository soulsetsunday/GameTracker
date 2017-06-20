using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameTracker.Controllers
{
    public class ChartController : Controller
    {
        private GameDbContext context;

        public ChartController(GameDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Month = null;
            IList<Game> games = context.Games.Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.DaysPlayed).ToList();

            return View(games);
        }

        //this could be passed a year, undecided on int or datetime
        //also, either change route or rename to id
        public IActionResult AllMonths(int passedYear = 0)
        {
            if (passedYear == 0)
                passedYear = DateTime.Today.Year;

            List<Day> days = context.Days.Where(d => d.CalendarDate.Year == passedYear).Include(g => g.GamesPlayed).OrderByDescending(x => x.CalendarDate).ToList();

            return View(days);
        }

        public IActionResult Monthly(string id)
        {
            DateTime sentCalendarDate = new DateTime();
            DateTime.TryParseExact(id, "MM-yyyy", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out sentCalendarDate);
            ViewBag.Month = sentCalendarDate.ToString("MMMM");

            List<int> sendingGameIDs = new List<int>();

            List<Day> days = context.Days.Where(d => d.CalendarDate.Year == sentCalendarDate.Year && d.CalendarDate.Month == sentCalendarDate.Month).Include(g => g.GamesPlayed).OrderByDescending(x => x.CalendarDate).ToList();
            foreach (var day in days)
            {
                foreach (var game in day.GamesPlayed)
                {
                    if (!sendingGameIDs.Contains(game.ID))
                    {
                        sendingGameIDs.Add(game.ID);
                    }
                }
            }

            IList<Game> games = context.Games.Where(g => sendingGameIDs.Contains(g.ID)).Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.DaysPlayed).ToList();

            return View("Index", games);
        }
        //public IActionResult Monthly()
        //{
        //    Calendar myCal = CultureInfo.InvariantCulture.Calendar;
        //    List<Day> days = context.Days.Include(g => g.GamesPlayed).OrderByDescending(x => x.CalendarDate).ToList();
        //    List<Month> months = new List<Month>();
        //    Dictionary<DateTime, int> monthDict = new Dictionary<DateTime, int>();

        //    foreach (var i in days)
        //    {
        //        DateTime testDate = new DateTime(myCal.GetYear(i.CalendarDate), myCal.GetMonth(i.CalendarDate), 1);
        //        if (!monthDict.ContainsKey(testDate))
        //        {
        //            monthDict.Add(testDate, 0);
        //        }
        //    }

        //    foreach (var i in days)
        //    {
        //        if i.CalendarDate.ToString()
        //    }

        //    return View(days);
        //}
    }
}
