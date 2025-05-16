using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPageMonitor.Core.Models
{
    /// <summary>
    /// Представляет изменения погодных данных, таких как температура, скорость ветра и условия.
    /// </summary>
    public class WeatherChange
    {
        public WeatherData? OldData { get; set; }
        public WeatherData? NewData { get; set; }
    }
}
