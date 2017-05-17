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

        public SearchController(GameDbContext dbContext)
        {
            context = dbContext;
        }

        private string url = "http://headers.jsontest.com/";
        private string privateapikey = "figureoutlater";
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

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(string id)
        {
            //id is the calendar date
            DateTime calendarDate = new DateTime();
            DateTime.TryParseExact(id, "M-dd-yyyy", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out calendarDate);
            ViewBag.date = calendarDate;
                //This should get a list of all games and platforms from database
                IList<Game> games = context.Games.Include(c => c.Platform).Include(i => i.GameImages).ToList();
            int checkgamesstate = 4;
            return View();
        }

        [HttpPost]
        public IActionResult Results(string searchstring)
        {
            ViewBag.Searchstring = searchstring;
            // Temporary thing
            RootObject searchResults = LoadJson();
            // Take search results, strip games out, make list
            //GameViewModelWrapper wrap = new GameViewModelWrapper();
            //List<GameViewModel> gameList = MakeGameList(searchResults);
            //TempData["GVM"] = MakeGameList(searchResults);
            //TODO: Remove MakeGameList and have view and static thing use rootobject
            //Maybe just send the list of results from rootobject, actual rootobject doesn't seem that useful
            //wrap.Viewmodels = MakeGameList(searchResults);
            resultList = searchResults.Results;

            return View("Index", resultList);
        }

        [HttpPost]
        public IActionResult AddGame(int gameid, int platformid)
        {
            //no model valedation, maybe later
            //all of this should be eslewhere
            //TODO: check if game is already in database

                for (int i=0; i < resultList.Count; i++ )
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
                            FirstAdded = DateTime.Today,
                            MostRecentlyAdded = DateTime.Today,

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
                    AddGameToDay(context.Games.Single(c => c.Name == resultList[i].Name));

                   
                }
                }


            IList<Game> games = context.Games.Include(i => i.GameImages).Include(p => p.Platform).ToList();

            return View(games);
        }

        public IActionResult AddGame()
        {
            IList<Game> games = context.Games.Include(i => i.GameImages).Include(p => p.Platform).ToList();
            int checkwhatgameslooklike = 4;
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
                context.SaveChanges();
            }
            else
            { 
                Day oldDay = context.Days.Single(d => d.CalendarDate == day);
                oldDay.GamesPlayed.Add(game);
            }


            return;
        }

        private async Task<String> GetTestObjects(string searchstring)
        {
            string url2 = $@"http://www.giantbomb.com/api/search/?api_key={privateapikey}&format=json&query='{searchstring}'&resources=game";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Game tracking demo thing");
            var response = await httpClient.GetAsync(url2);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }


        public RootObject LoadJson()
        {
            using (StreamReader r = System.IO.File.OpenText("/Data/file2.json"))
            {
                string json = r.ReadToEnd();
                RootObject items = JsonConvert.DeserializeObject<RootObject>(json);
                return items;
            }
        }

        //pass rootobject, return a list of games
        //this is a list of viewmodels plus a list of platforms
        //the rootobject.result(i.e. the game) comes with a list of platforms
        //No idea why I wanted to do this, just use the rootobject
        //The notable thing it did was parse the release date into a DateTime
        public List<GameViewModel> MakeGameList(RootObject root)
        {
            List<GameViewModel> gList = new List<GameViewModel>();
            foreach (var g in root.Results)
            {

                GameViewModel ng = new GameViewModel
                {
                    Name = g.Name,
                    Original_release_date = DateTime.Parse(g.Original_release_date),
                    //Image = g.Image,
                    Platforms = g.Platforms
                };
                gList.Add(ng);


            }
            return gList;
        }
    } 
}
