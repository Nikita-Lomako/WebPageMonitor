using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Core.Models
{
    public class TimeSlotData
    {
        public string Time { get; set; } = string.Empty;
        public string Temperature { get; set; } = string.Empty;
        public string WindSpeed { get; set; } = string.Empty;
        public string WindDirection { get; set; } = string.Empty;
        public string Humidity { get; set; } = string.Empty;
        public string Pressure { get; set; } = string.Empty;
        public string Precipitation { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is not TimeSlotData other)
                return false;

            return Time == other.Time &&
                   Temperature == other.Temperature &&
                   WindSpeed == other.WindSpeed &&
                   WindDirection == other.WindDirection &&
                   Humidity == other.Humidity &&
                   Pressure == other.Pressure &&
                   Precipitation == other.Precipitation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Time, Temperature, WindSpeed, WindDirection, Humidity, Pressure, Precipitation);
        }
    }
}
