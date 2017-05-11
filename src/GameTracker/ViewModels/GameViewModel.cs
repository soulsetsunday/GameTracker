using GameTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameTracker.ViewModels
{
    public class GameViewModel
    {
        public string Name { get; set; }
        public DateTime Original_release_date { get; set; }
        public int ID { get; set; }
        public Image Image { get; set; }
        public List<Platform> Platforms { get; set; }
    }

    public class GameViewModelWrapper
    {
        public List<GameViewModel> Viewmodels { get; set; }
        public Platform Platform { get; set; }
        public Game Game { get; set; }

        public GameViewModelWrapper()
        {
            Platform Platform = new Models.Platform();
            Game Game = new Models.Game();
        }
    }
}
