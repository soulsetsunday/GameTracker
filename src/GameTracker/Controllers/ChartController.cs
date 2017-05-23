using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

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
            IList<Game> games = context.Games.Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.DaysPlayed).ToList();

            return View(games);
        }
    }
}
