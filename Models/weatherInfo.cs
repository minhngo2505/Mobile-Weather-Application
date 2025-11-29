using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class WeatherInfo
    {
        public string City { get; set; } = string.Empty;
        public double TempC { get; set; }
        public double FeelsLikeC { get; set; }
        public int Humidity { get; set; }
        public double WindKph { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string LocalTime { get; set; } = string.Empty;
        public double Lat { get; set; }
        public double Lon { get; set; }



    }
}
