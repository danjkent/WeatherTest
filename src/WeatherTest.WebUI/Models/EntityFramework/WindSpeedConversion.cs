using System;
using System.Collections.Generic;

namespace WeatherTest.WebUI.Models
{
    public partial class WindSpeedConversion
    {
        public int Id { get; set; }
        public int FromWindSpeedUomid { get; set; }
        public int ToWindSpeedUomid { get; set; }
        public string Formula { get; set; }

        public virtual WindSpeedUom FromWindSpeedUom { get; set; }
        public virtual WindSpeedUom ToWindSpeedUom { get; set; }
    }
}
