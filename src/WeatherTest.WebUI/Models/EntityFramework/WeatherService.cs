using System;
using System.Collections.Generic;

namespace WeatherTest.WebUI.Models
{
    public partial class WeatherService
    {
        public int Id { get; set; }
        public int TemperatureUomid { get; set; }
        public int WindSpeedUomid { get; set; }
        public string Title { get; set; }
        public string ServiceAddress { get; set; }
        public string TemperaturePath { get; set; }
        public string WindSpeedPath { get; set; }

        public virtual TemperatureUom TemperatureUom { get; set; }
        public virtual WindSpeedUom WindSpeedUom { get; set; }
    }
}
