using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameTracker.Models
{
    public class XmlGame
    {
        [XmlRoot(ElementName = "image")]
        public class Image
        {
            [XmlElement(ElementName = "icon_url")]
            public string Icon_url { get; set; }
            [XmlElement(ElementName = "medium_url")]
            public string Medium_url { get; set; }
            [XmlElement(ElementName = "screen_url")]
            public string Screen_url { get; set; }
            [XmlElement(ElementName = "small_url")]
            public string Small_url { get; set; }
            [XmlElement(ElementName = "super_url")]
            public string Super_url { get; set; }
            [XmlElement(ElementName = "thumb_url")]
            public string Thumb_url { get; set; }
            [XmlElement(ElementName = "tiny_url")]
            public string Tiny_url { get; set; }
        }

        [XmlRoot(ElementName = "game")]
        public class Game
        {
            [XmlElement(ElementName = "image")]
            public Image Image { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "original_release_date")]
            public string Original_release_date { get; set; }
            [XmlElement(ElementName = "resource_type")]
            public string Resource_type { get; set; }
        }

        [XmlRoot(ElementName = "results")]
        public class Results
        {
            [XmlElement(ElementName = "game")]
            public List<Game> Game { get; set; }

            public Results()
            {
                Game = new List<XmlGame.Game>();
            }
        }

        [XmlRoot(ElementName = "response")]
        public class Response
        {
            [XmlElement(ElementName = "error")]
            public string Error { get; set; }
            [XmlElement(ElementName = "limit")]
            public string Limit { get; set; }
            [XmlElement(ElementName = "offset")]
            public string Offset { get; set; }
            [XmlElement(ElementName = "number_of_page_results")]
            public string Number_of_page_results { get; set; }
            [XmlElement(ElementName = "number_of_total_results")]
            public string Number_of_total_results { get; set; }
            [XmlElement(ElementName = "status_code")]
            public string Status_code { get; set; }
            [XmlElement(ElementName = "results")]
            public Results Results { get; set; }
            [XmlElement(ElementName = "version")]
            public string Version { get; set; }
        }
    }
}
