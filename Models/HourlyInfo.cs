using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class HourlyInfo
    {
        public string Time { get; set; } = string.Empty;
        public double TempC { get; set; }
        public string IconUrl { get; set; } = string.Empty;
    }
}
