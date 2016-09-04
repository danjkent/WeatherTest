using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherTest.WebUI.Models
{
    public class HomeViewModel : Disposable
    {
        #region Constants

        #endregion

        #region Constructors

        #endregion

        #region Events + Delegates

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the location to be searched
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the current results of the search
        /// </summary>
        public QueryResults Results { get; set; }

        /// <summary>
        /// Gets or sets the selected unit of measure for temperature
        /// </summary>
        [Display(Name="Temperature Units")]
        public int? SelectedTemperatureUnits { get; set; }

        /// <summary>
        /// Gets or sets the selected unit of measure for wind speed
        /// </summary>
        [Display(Name = "Wind Speed Units")]
        public int? SelectedWindSpeedUnits { get; set; }

        /// <summary>
        /// Gets or sets the available units of measure for temperature
        /// </summary>
        public SelectList TemperatureUnits { get; set; }

        /// <summary>
        /// Gets or sets the available units of measure for wind speed
        /// </summary>
        public SelectList WindSpeedUnits { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
