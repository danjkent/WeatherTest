using System;
using System.Collections.Generic;

namespace WeatherTest.WebUI.Models
{
    public partial class WindSpeedUom
    {
        public WindSpeedUom()
        {
            WeatherService = new HashSet<WeatherService>();
            WindSpeedConversionFromWindSpeedUom = new HashSet<WindSpeedConversion>();
            WindSpeedConversionToWindSpeedUom = new HashSet<WindSpeedConversion>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<WeatherService> WeatherService { get; set; }
        public virtual ICollection<WindSpeedConversion> WindSpeedConversionFromWindSpeedUom { get; set; }
        public virtual ICollection<WindSpeedConversion> WindSpeedConversionToWindSpeedUom { get; set; }
    }
}
