using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameTracker.Models
{
    public class Day
    {
        public int ID { get; set; }
        public DateTime CalendarDate { get; set; }
        public List<Game> GamesPlayed { get; set; }

        public Day()
        {
            GamesPlayed = new List<Game>();
        }
    }

    //public class Month
    //{
    //    public DateTime StartOfMonth { get; set; }
    //    public int GamesInMonth { get; set; }
    //}
}
