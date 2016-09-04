using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherTest.WebUI.Models
{
    [Serializable]
    public class QueryResults
    {
        #region Constants

        #endregion

        #region Constructors

        #endregion

        #region Events + Delegates

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the number of services that could not be connected to
        /// </summary>
        public int ServicesOffline { get; set; }

        /// <summary>
        /// Gets or sets the calculated average temperature
        /// </summary>
        public decimal TemperatureResult { get; set; }

        /// <summary>
        /// Gets or sets the selected temperature UOM description
        /// </summary>
        public string TemperatureUOM { get; set; }

        /// <summary>
        /// Gets or sets the number of services to be queried
        /// </summary>
        public int TotalServices { get; set; }

        /// <summary>
        /// Gets or sets the calculated average wind speed
        /// </summary>
        public decimal WindSpeedResult { get; set; }

        /// <summary>
        /// Gets or sets the selected wind speed UOM description
        /// </summary>
        public string WindSpeedUOM { get; set; }

        #endregion

        #region Methods
        public override string ToString()
        {
            return string.Format("Temperature: {0:0.0}° ({1}). Wind Speed: {2:0.0} ({3}). ({4} Sources, {5} Offline)",
                        TemperatureResult,
                        TemperatureUOM,
                        WindSpeedResult,
                        WindSpeedUOM,
                        TotalServices,
                        ServicesOffline);
        }
        #endregion

    }
}
