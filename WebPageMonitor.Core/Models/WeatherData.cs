using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Core.Models
{
    /// <summary>
    /// Представляет данные о погоде, такие как температура, скорость ветра и погодные условия.
    /// </summary>
    public class WeatherData
    {
        public List<TimeSlotData> TimeSlots { get; set; } = new List<TimeSlotData>();
        public DateTime ObservationTime { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not WeatherData other)
                return false;

            if (TimeSlots.Count != other.TimeSlots.Count)
                return false;

            for (int i = 0; i < TimeSlots.Count; i++)
            {
                if (!TimeSlots[i].Equals(other.TimeSlots[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeSlots, ObservationTime);
        }

        public string ToDisplayString()
        {
            return $"Время наблюдения: {ObservationTime:G}";
        }
    }
}

