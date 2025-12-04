using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class TenDayInfo
    {
        public String Day { get; set; } = "";
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public String Condition { get; set; } = "";
        public String IconUrl { get; set; } = "";
        public string MaxTempDisplay => FormatTemp(MaxTemp);
        public string MinTempDisplay => FormatTemp(MinTemp);

        private static string FormatTemp(double tempC)
        {
            string unit = Preferences.Get("units", "C");
            double value = unit == "C" ? tempC : (tempC * 9 / 5) + 32;
            return unit == "C" ? $"{value:0.#}°C" : $"{value:0.#}°F";
        }
    }
}
