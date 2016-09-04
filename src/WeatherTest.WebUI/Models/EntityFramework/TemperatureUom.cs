using System;
using System.Collections.Generic;

namespace WeatherTest.WebUI.Models
{
    public partial class TemperatureUom
    {
        public TemperatureUom()
        {
            TemperatureConversionFromTemperatureUom = new HashSet<TemperatureConversion>();
            TemperatureConversionToTemperatureUom = new HashSet<TemperatureConversion>();
            WeatherService = new HashSet<WeatherService>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<TemperatureConversion> TemperatureConversionFromTemperatureUom { get; set; }
        public virtual ICollection<TemperatureConversion> TemperatureConversionToTemperatureUom { get; set; }
        public virtual ICollection<WeatherService> WeatherService { get; set; }
    }
}
