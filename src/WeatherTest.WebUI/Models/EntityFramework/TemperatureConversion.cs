using System;
using System.Collections.Generic;

namespace WeatherTest.WebUI.Models
{
    public partial class TemperatureConversion
    {
        public int Id { get; set; }
        public int FromTemperatureUomid { get; set; }
        public int ToTemperatureUomid { get; set; }
        public string Formula { get; set; }

        public virtual TemperatureUom FromTemperatureUom { get; set; }
        public virtual TemperatureUom ToTemperatureUom { get; set; }
    }
}
