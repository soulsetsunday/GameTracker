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
            //ViewBag.Month = null;
            //ViewBag.Year = null;
            IList<Game> games = context.Games.Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.DaysPlayed).ToList();

            return View(games);
        }

        public IActionResult AllMonths(int id = 0)
        {
            int passedYear;
            if (id == 0)
                passedYear = DateTime.Today.Year;
            else
                passedYear = id;

            List<Day> days = context.Days.Where(d => d.CalendarDate.Year == passedYear).Include(g => g.GamesPlayed).OrderByDescending(x => x.CalendarDate).ToList();

            return View(days);
        }

        public IActionResult AllYears()
        {
            //I don't know that these need to be stings
            List<string> yearList = new List<string>();
            List<Day> days = context.Days.Include(g => g.GamesPlayed).OrderByDescending(x => x.CalendarDate).ToList();

            foreach (var day in days)
                if (!yearList.Contains(Convert.ToString(day.CalendarDate.Year)))
                    yearList.Add(Convert.ToString(day.CalendarDate.Year));

            return View(yearList);

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

        public IActionResult Yearly(string id)
        {
            DateTime sentCalendarDate = new DateTime();
            DateTime.TryParseExact(id, "yyyy", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out sentCalendarDate);
            ViewBag.Year = sentCalendarDate.ToString("yyyy");

            List<int> sendingGameIDs = new List<int>();

            List<Day> days = context.Days.Where(d => d.CalendarDate.Year == sentCalendarDate.Year).Include(g => g.GamesPlayed).OrderByDescending(x => x.CalendarDate).ToList();
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

        public IActionResult Goty()
        {
            IList<Game> games = context.Games.Where(g => g.Original_release_date.Year == DateTime.Today.Year).Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.DaysPlayed).ToList();
            return View("Index", games);
        }
    }
}
