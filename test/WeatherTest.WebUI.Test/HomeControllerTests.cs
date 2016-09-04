using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherTest.WebUI.Controllers;
using WeatherTest.WebUI.Models;
using Xunit;

namespace WeatherTest.WebUI.Test
{
    public class HomeControllerTests
    {
        #region Constants
        private const string TestLocation = "Bournemouth";

        #endregion

        #region Constructors
        public HomeControllerTests()
        {
            DbContextOptionsBuilder<WeatherTestContext> optionsBuilder = new DbContextOptionsBuilder<WeatherTestContext>();
            optionsBuilder.UseInMemoryDatabase();
            _weatherTestDb = new WeatherTestContext(optionsBuilder.Options);

            DefaultData.Create(_weatherTestDb);

            _homeController = new HomeController(_weatherTestDb);

            _celsius = WeatherTestDb.TemperatureUom.First(i => i.Title == DefaultData.CelsiusTitle);
            _fahrenheit = WeatherTestDb.TemperatureUom.First(i => i.Title == DefaultData.FahrenheitTitle);

            _kph = WeatherTestDb.WindSpeedUom.First(i => i.Title == DefaultData.KPHTitle);
            _mph = WeatherTestDb.WindSpeedUom.First(i => i.Title == DefaultData.MPHTitle);
        }

        #endregion

        #region Events + Delegates

        #endregion

        #region Properties
        private HomeController _homeController = null;
        private WeatherTestContext _weatherTestDb = null;

        TemperatureUom _celsius = null;
        TemperatureUom _fahrenheit = null;

        WindSpeedUom _kph = null;
        WindSpeedUom _mph = null;

        /// <summary>
        /// Gets the reference to the data context
        /// </summary>
        public WeatherTestContext WeatherTestDb
        {
            get
            {
                return _weatherTestDb;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Given temperatures of 10c from bbc and 68f from accuweather when searching then display 15° based on a user choosing Celsius as the temperature UOM
        /// </summary>
        [Fact]
        public void Temperature_Celsius_Test()
        {
            QueryResults result = _homeController.QueryServices(_celsius.Id, _kph.Id, TestLocation, new HomeController.QueryServiceDelegate(QueryService)).Result;
            Assert.Equal(15, result.TemperatureResult);
        }

        /// <summary>
        /// Given temperatures of 10c from bbc and 68f from accuweather when searching then display 59° based on a user choosing Fahrenheit as the temperature UOM
        /// </summary>
        [Fact]
        public void Temperature_Fahrenheit_Test()
        {
            QueryResults result = _homeController.QueryServices(_fahrenheit.Id, _mph.Id, TestLocation, new HomeController.QueryServiceDelegate(QueryService)).Result;
            Assert.Equal(59, result.TemperatureResult);
        }

        /// <summary>
        /// Given wind speeds of 8kph from bbc and 10mph from accuweather when searching then display 12kph based on a user choosing KPH as the wind-speed UOM
        /// </summary>
        [Fact]
        public void WindSpeed_KPH_Test()
        {
            QueryResults result = _homeController.QueryServices(_celsius.Id, _kph.Id, TestLocation, new HomeController.QueryServiceDelegate(QueryService)).Result;
            Assert.Equal(12, result.WindSpeedResult);
        }

        /// <summary>
        /// Given wind speeds of 8kph from bbc and 10mph from accuweather when searching then display either 7.5mph based on a user choosing MPH as the wind-speed UOM
        /// </summary>
        [Fact]
        public void WindSpeed_MPH_Test()
        {
            QueryResults result = _homeController.QueryServices(_fahrenheit.Id, _mph.Id, TestLocation, new HomeController.QueryServiceDelegate(QueryService)).Result;
            Assert.Equal(7.5m, result.WindSpeedResult);
        }

        /// <summary>
        /// Test implementation of service querying
        /// </summary>
        /// <param name="service">The service to query</param>
        /// <param name="location">The location to search for</param>
        public async Task<ServiceResult> QueryService(WeatherService service, string location)
        {
            ServiceResult returnValue = new ServiceResult();

            if (service.Title == DefaultData.AccuWeatherTitle)
            {
                returnValue.TemperatureResult = 68;
                returnValue.WindSpeedResult = 10;
            }
            else if (service.Title == DefaultData.BBCTitle)
            {
                returnValue.TemperatureResult = 10;
                returnValue.WindSpeedResult = 8;
            }

            await Task.Delay(0);

            return returnValue;
        }

        #endregion

    }
}
