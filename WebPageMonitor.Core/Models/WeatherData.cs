using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Core.Models
{
    public class WeatherData
    {
        public string Location { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string WindSpeed { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public DateTime ObservationTime { get; set; }
    }
}

