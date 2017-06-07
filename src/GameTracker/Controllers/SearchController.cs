using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameTracker.Data;
using GameTracker.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using GameTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Globalization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GameTracker.Controllers
{
    public class SearchController : Controller
    {
        private GameDbContext context;
        private static readonly HttpClient HttpClient = new HttpClient();

        public SearchController(GameDbContext dbContext)
        {
            context = dbContext;
        }

        private const string apiFileLocation = "/Data/apikey.txt";
        private string privateapikey = LoadAPI();
        public static List<Game> mainGameList = new List<Game>();
        public static GameViewModelWrapper wrap = new GameViewModelWrapper();
        //static to hopefully pass values around
        public static List<Result> resultList = new List<Result>();
        //try 2, static to pass around controller, list of results from rootobject
        public static Platform tempPlatform = new Platform();
        //Really bad form, probably 
        public static int storeIndex;
        //"
        public static DateTime dateForDB;
        public static DateTime currentWorkingDate;
        public const int mostRecentlyAddedLimit = 5;

        [HttpGet]
        public IActionResult Index(string id)
        {
            //id is the date in a string, it's named id because of routing, maybe look up how to change it
            DateTime calendarDate = new DateTime();
            DateTime.TryParseExact(id, "MM-dd-yyyy", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out calendarDate);
            //TODO: if this is null, make it today
            ViewBag.date = calendarDate;
            currentWorkingDate = calendarDate;

            IList<Game> games = context.Games.Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.MostRecentlyAdded).Take(mostRecentlyAddedLimit).ToList();
            return View(games);
        }

        [HttpPost]
        public async Task<IActionResult> Results(string searchstring)
        {
            ViewBag.Searchstring = searchstring;
            // Temporary thing
            //RootObject searchResults = LoadJson();

            //actual loading
            string response = await SendSearchRequest(searchstring);
            RootObject searchResults = JsonConvert.DeserializeObject<RootObject>(response);

            return View(searchResults);
        }

        [HttpPost]
        public IActionResult AddGame(int gameid, int platformid)
        {
            //no model valedation, maybe later
            //all of this should be eslewhere
            //TODO: check if game is already in database

            for (int i = 0; i < resultList.Count; i++)
            {
                if (resultList[i].id == gameid)
                {
                    storeIndex = i;

                    int loopcheck1 = 0;
                    var loopcheck2 = resultList[i].Platforms.Count;
                    for (int j = 0; j < resultList[i].Platforms.Count; j++)
                    {
                        var loopcheck3 = resultList[i].Platforms[j].ID;
                        if (resultList[i].Platforms[j].ID == platformid)
                        {
                            //SingleOrDefault instead of single to prevent null exceptions
                            var testing2 = context.Platforms;
                            //var testing = context.Platforms.FirstOrDefault(c => c.Name == resultList[i].Platforms[j].Name).Name;
                            var testing3 = resultList[i].Platforms[j].Name;
                            var testing5 = context.Platforms.Any(s => s.Name == testing3);
                            //var testing4 = context.Platforms.SingleOrDefault(c => c.Name == resultList[i].Platforms[j].Name).Name;
                            //if (context.Platforms.SingleOrDefault(c => c.Name == resultList[i].Platforms[j].Name).Name == null || resultList[i].Platforms[j].Name != context.Platforms.SingleOrDefault(c => c.Name == resultList[i].Platforms[j].Name).Name)
                            if (!context.Platforms.Any(s => s.Name == resultList[i].Platforms[j].Name))
                            {
                                Platform newPlatform = new Platform
                                {
                                    Name = resultList[i].Platforms[j].Name,
                                };
                                context.Platforms.Add(newPlatform);
                                context.SaveChanges();
                                //figure out how to do this
                                tempPlatform = context.Platforms.Single(c => c.Name == resultList[i].Platforms[j].Name);
                            }
                            else
                            {
                                tempPlatform = context.Platforms.Single(c => c.Name == resultList[i].Platforms[j].Name);
                            };
                        }
                    }
                    //end of platform part

                    Game newDBGame = new Game
                    {
                        Name = resultList[i].Name,
                        Original_release_date = DateTime.Parse(resultList[i].Original_release_date),
                        Platform = tempPlatform,
                        //These should probably be in addgametoday
                        FirstAdded = currentWorkingDate,
                        MostRecentlyAdded = currentWorkingDate,

                    };

                    Type type = typeof(Image);
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        var gv = resultList[i].Image;
                        var altv = resultList[i].Image.GetType().GetProperty("Icon_url").GetValue(gv, null);
                        var tryname = property.Name;
                        var svalue2 = resultList[i].Image.GetType().GetProperty(tryname).GetValue(gv, null);
                        //var svalue = resultList[i].Image.GetType().GetProperty("property.Name").GetValue(gv, null);
                        newDBGame.GameImages.GetType().GetProperty(property.Name).SetValue(newDBGame.GameImages, svalue2);
                    }

                    context.Games.Add(newDBGame);
                    context.SaveChanges();
                    //add the game to a day
                    AddGameToDay(context.Games.Single(c => c.Name == resultList[i].Name), currentWorkingDate);


                }
            }


            IList<Game> games = context.Games.Include(i => i.GameImages).Include(p => p.Platform).ToList();

            return View(games);
        }

        public IActionResult AddGame()
        {
            //this was for early testing, after adding a game maybe redirect to chart
            IList<Game> games = context.Games.Include(i => i.GameImages).Include(p => p.Platform).ToList();
            return View(games);
        }

        [HttpPost]
        public IActionResult AddRecentGame(int gameid)
        {
            Game recentGame = context.Games.Single(c => c.ID == gameid);
            AddGameToDay(recentGame, currentWorkingDate);

            //this could probably go to a stats page or something
            //return AddGame();
            return View("AddGame");

        }

        public IActionResult AllGames()
        {
            IList<Game> games = context.Games.Include(c => c.Platform).Include(i => i.GameImages).OrderByDescending(x => x.MostRecentlyAdded).ToList();
            return View(games);
        }

        public IActionResult Days()
        {
            List<Day> days = context.Days.Include(g => g.GamesPlayed).ToList();
            return View(days);
        }

        //this needs to be elsewhere
        //can't pass DateTime.Today, nullable DateTime would need to be converted 
        //this should take other days eventually
        public void AddGameToDay(Game game, DateTime day = default(DateTime))
        {
            //adding to a day adds to days played, this makes some assumptions
            game.DaysPlayed++;

            if (day == default(DateTime))
            {
                day = DateTime.Today;
            }
            if (!context.Days.Any(d => d.CalendarDate == day))
            {
                Day newDay = new Day();

                newDay.CalendarDate = day;
                newDay.GamesPlayed.Add(game);


                context.Days.Add(newDay);
            }
            else
            {
                Day oldDay = context.Days.Single(d => d.CalendarDate == day);
                oldDay.GamesPlayed.Add(game);
                //under the assumption that this will eventually take dates other than today
                if (game.MostRecentlyAdded < day)
                {
                    game.MostRecentlyAdded = day;
                }

            }

            //I hope this can be outside the loops?
            context.SaveChanges();
            return;
        }

        //this is a total mess
        private async Task<String> SendSearchRequest(string searchstring, int offset = 0)
        {
            string url2 = $@"http://www.giantbomb.com/api/search/?api_key={privateapikey}&format=json&query='{searchstring}'&resources=game";
            //per documentation, use one static httpclient per app
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Game tracking demo thing");
            var response = await HttpClient.GetAsync(url2);
            var result = await response.Content.ReadAsStringAsync();
            ViewBag.urltest = url2;

            return result;
        }



        //the file locations here will be an issue
        static string LoadAPI()
        {
            using (StreamReader r = System.IO.File.OpenText(apiFileLocation))
            {
                string api = r.ReadToEnd();
                return api;
            }
        }

        //loads test json files to parse
        public RootObject LoadJson()
        {
            using (StreamReader r = System.IO.File.OpenText("/Data/eternal.json"))
            {
                string json = r.ReadToEnd();
                RootObject items = JsonConvert.DeserializeObject<RootObject>(json);
                return items;
            }
        }

        //this is a debugging thing, remove
        public void TestJson()
        {
            RootObject test = LoadJson();
            int i = 4;
            return;

        }
    } 
}
