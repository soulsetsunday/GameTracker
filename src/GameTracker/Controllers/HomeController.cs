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
        private GameDbContext context;

        public HomeController(GameDbContext dbContext)
        {
            context = dbContext;
        }

        private string url = "http://headers.jsontest.com/";
        private string privateapikey = "9096f0658a958915fe37842c9eb07aa0a79380f1";
        public static List<Game> mainGameList = new List<Game>();

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

        public IActionResult SearchBox()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchBox(string searchstring)
        {
            ViewBag.Searchstring = searchstring;
            //ViewBag.Json = GetTestObjects(searchstring).Result;
            ViewBag.Items = LoadJson();
            // Temporary thing
            RootObject searchResults = LoadJson();
            // Take search results, strip games out, make list
            List<Game> gameList = MakeGameList(searchResults);

            // Actual search code, commented out for now
            // Sends search, returns raw json
            //var rawsearchresults = GetTestObjects(searchstring).Result;
            // parses json
            //RootObject searchitems = JsonConvert.DeserializeObject<RootObject>(rawsearchresults);

            //return View(searchResults);
            return View(gameList);
        }
        [HttpPost]
        public IActionResult AddGame(Game newgame)
        {
            mainGameList.Add(newgame);
            //dbupdate probably goes here
            return View("DisplayGames");
        }

        public IActionResult DisplayGames(List<Game> games)
        {
            return View(mainGameList);
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
        public List<Game> MakeGameList(RootObject root)
        {
            List<Game> gList = new List<Game>();
            foreach (var g in root.Results)
            {
                
                //Game ng = new Game { Name = g.Name,
                //    Original_release_date = DateTime.Parse(g.Original_release_date),
                //    Images = g.Image };
                //gList.Add(ng);
            }

            return gList;
        }
    }
}
