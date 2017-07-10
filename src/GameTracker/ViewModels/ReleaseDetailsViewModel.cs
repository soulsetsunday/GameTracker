using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameTracker.ViewModels
{
    public class ReleaseDetailsViewModel
    {
        public int GameID { get; set; }
        public int PlatformID { get; set; }
        public string ViewDate { get; set; }
        public List<string> ReleaseDates { get; set; }
    }
}
