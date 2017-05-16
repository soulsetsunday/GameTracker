using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameTracker.Models
{
    public class Game
    {
        public string Name { get; set; }
        public DateTime Original_release_date { get; set; }
        public int ID { get; set; }
        public GameImage GameImages { get; set; }
        public DateTime FirstAdded { get; set; }
        public DateTime MostRecentlyAdded { get; set; }
        public int DaysPlayed { get; set; } = 1;
        //not great, but atm games are only added if played on a day

        public Platform Platform { get; set; }


        //TODO: display platforms; this needs to be a single platform picked from the view
        public Game()
        {
            GameImages = new GameImage();
        }
            
    }

    public class Day
    {
        public int ID { get; set; }
        public DateTime CalendarDate { get; set; }
        public IList<Game> GamesPlayed { get; set; }
    }

    public class GameImage
    {
        public string Icon_url { get; set; }
        public string Medium_url { get; set; }
        public string Screen_url { get; set; }
        public string Small_url { get; set; }
        public string Super_url { get; set; }
        public string Thumb_url { get; set; }
        public string Tiny_url { get; set; }
        public int ID { get; set; }
    }

    public class Image
    {
        public string Icon_url { get; set; }
        public string Medium_url { get; set; }
        public string Screen_url { get; set; }
        public string Small_url { get; set; }
        public string Super_url { get; set; }
        public string Thumb_url { get; set; }
        public string Tiny_url { get; set; }
    }
    public class OriginalGameRating
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Platform
    {
        public string api_detail_url { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string site_detail_url { get; set; }
        public string abbreviation { get; set; }

        public IList<Game> Games { get; set; }
    }

    public class Result
    {
        public string aliases { get; set; }
        public string api_detail_url { get; set; }
        public string date_added { get; set; }
        public string date_last_updated { get; set; }
        public string deck { get; set; }
        public string description { get; set; }
        public object expected_release_day { get; set; }
        public object expected_release_month { get; set; }
        public object expected_release_quarter { get; set; }
        public object expected_release_year { get; set; }
        public int id { get; set; }
        public Image Image { get; set; }
        public string Name { get; set; }
        public int number_of_user_reviews { get; set; }
        public List<OriginalGameRating> original_game_rating { get; set; }
        public string Original_release_date { get; set; }
        public List<Platform> Platforms { get; set; }
        public string site_detail_url { get; set; }
        public string Resource_type { get; set; }
    }

    public class RootObject
    {
        public string error { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int number_of_page_results { get; set; }
        public int number_of_total_results { get; set; }
        public int status_code { get; set; }
        public List<Result> Results { get; set; }
        public string version { get; set; }

        public RootObject()
        {
            Results = new List<Result>();
        }
    }

}