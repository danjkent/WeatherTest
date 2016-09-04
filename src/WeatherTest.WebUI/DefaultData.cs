using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WeatherTest.WebUI.Models;


namespace WeatherTest.WebUI
{
    public class DefaultData
    {
        #region Constants
        public const string AccuWeatherTitle = "AccuWeather";
        public const string BBCTitle = "BBC";

        public const string CelsiusTitle = "Celsius";
        public const string FahrenheitTitle = "Fahrenheit";

        public const string KPHTitle = "KPH";
        public const string MPHTitle = "MPH";

        #endregion

        #region Constructors

        #endregion

        #region Events + Delegates

        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Ensures the database's default data has been initialised
        /// </summary>
        /// <param name="serviceProvider">Configuration options</param>
        public static void Create(IServiceProvider serviceProvider)
        {
            using (WeatherTestContext weatherDb = new WeatherTestContext(serviceProvider.GetRequiredService<DbContextOptions<WeatherTestContext>>()))
            {
                Create(weatherDb);
            }
        }

        /// <summary>
        /// Ensures the database's default data has been initialised
        /// </summary>
        /// <param name="weatherDb">The database to be populated</param>
        public static void Create(WeatherTestContext weatherDb)
        {
            if (weatherDb.TemperatureConversion.Any() ||
                    weatherDb.TemperatureUom.Any() ||
                    weatherDb.WeatherService.Any() ||
                    weatherDb.WindSpeedConversion.Any() ||
                    weatherDb.WindSpeedUom.Any())
                    return;

            TemperatureUom celsius = new TemperatureUom() { Title = CelsiusTitle };
            TemperatureUom fahrenheit = new TemperatureUom() { Title = FahrenheitTitle };
            weatherDb.TemperatureUom.AddRange(celsius, fahrenheit);

            weatherDb.TemperatureConversion.Add(new TemperatureConversion() { FromTemperatureUom = celsius, ToTemperatureUom = fahrenheit, Formula = "{value}*(9/5)+32" });
            weatherDb.TemperatureConversion.Add(new TemperatureConversion() { FromTemperatureUom = fahrenheit, ToTemperatureUom = celsius, Formula = "({value}-32)*(5/9)" });

            WindSpeedUom mph = new WindSpeedUom() { Title = "MPH" };
            WindSpeedUom kph = new WindSpeedUom() { Title = "KPH" };
            weatherDb.WindSpeedUom.AddRange(mph, kph);

            weatherDb.WindSpeedConversion.Add(new WindSpeedConversion() { FromWindSpeedUom = mph, ToWindSpeedUom = kph, Formula = "{value}*1.6" });
            weatherDb.WindSpeedConversion.Add(new WindSpeedConversion() { FromWindSpeedUom = kph, ToWindSpeedUom = mph, Formula = "{value}/1.6" });

            WeatherService accu = new WeatherService()
            {
                Title = "AccuWeather",
                ServiceAddress = "http://localhost:60368/{location}",
                TemperaturePath = "temperatureFahrenheit",
                TemperatureUom = fahrenheit,
                WindSpeedPath = "windSpeedMph",
                WindSpeedUom = mph
            };
            WeatherService bbc = new WeatherService()
            {
                Title = "BBC",
                ServiceAddress = "http://localhost:60350/Weather/{location}",
                TemperaturePath = "temperatureCelsius",
                TemperatureUom = celsius,
                WindSpeedPath = "windSpeedKph",
                WindSpeedUom = kph
            };

            weatherDb.WeatherService.AddRange(accu, bbc);
            weatherDb.SaveChanges();
        }

        /// <summary>
        /// Creates the additional demo data for the test
        /// </summary>
        /// <param name="serviceProvider">Configuration options</param>
        public static void CreateDemoData(WeatherTestContext weatherDb)
        {
            if (weatherDb.WeatherService.Any(i => i.Title == "Yahoo"))
                return;

            TemperatureUom kelvin = new TemperatureUom() { Title = "Kelvin" };
            TemperatureUom celsius = weatherDb.TemperatureUom.First(i => i.Title == "Celsius");
            TemperatureUom fahrenheit = weatherDb.TemperatureUom.First(i => i.Title == "Fahrenheit");
            weatherDb.TemperatureUom.Add(kelvin);

            weatherDb.TemperatureConversion.Add(new TemperatureConversion() { FromTemperatureUom = kelvin, ToTemperatureUom = celsius, Formula = "({value}-273.15)" });
            weatherDb.TemperatureConversion.Add(new TemperatureConversion() { FromTemperatureUom = kelvin, ToTemperatureUom = fahrenheit, Formula = "({value}*(9/5)-459.67)" });
            weatherDb.TemperatureConversion.Add(new TemperatureConversion() { FromTemperatureUom = celsius, ToTemperatureUom = kelvin, Formula = "({value}+273.15)" });
            weatherDb.TemperatureConversion.Add(new TemperatureConversion() { FromTemperatureUom = fahrenheit, ToTemperatureUom = kelvin, Formula = "(({value} + 459.67)*(5/9))" });

            WindSpeedUom feetSecond = new WindSpeedUom() { Title = "ft/s" };
            WindSpeedUom mph = weatherDb.WindSpeedUom.First(i => i.Title == "MPH");
            WindSpeedUom kph = weatherDb.WindSpeedUom.First(i => i.Title == "KPH");
            weatherDb.WindSpeedUom.Add(feetSecond);

            weatherDb.WindSpeedConversion.Add(new WindSpeedConversion() { FromWindSpeedUom = feetSecond, ToWindSpeedUom = kph, Formula = "({value}*1.097)" });
            weatherDb.WindSpeedConversion.Add(new WindSpeedConversion() { FromWindSpeedUom = feetSecond, ToWindSpeedUom = mph, Formula = "({value}*0.682)" });
            weatherDb.WindSpeedConversion.Add(new WindSpeedConversion() { FromWindSpeedUom = kph, ToWindSpeedUom = feetSecond, Formula = "({value}/1.097)" });
            weatherDb.WindSpeedConversion.Add(new WindSpeedConversion() { FromWindSpeedUom = mph, ToWindSpeedUom = feetSecond, Formula = "({value}/0.682)" });

            WeatherService yahoo = new WeatherService()
            {
                Title = "Yahoo",
                ServiceAddress = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22{location}%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys",
                TemperaturePath = "query.results.channel.item.condition.temp",
                TemperatureUom = fahrenheit,
                WindSpeedPath = "query.results.channel.wind.speed",
                WindSpeedUom = mph
            };

            weatherDb.WeatherService.Add(yahoo);
            weatherDb.SaveChanges();
        }

        #endregion

    }
}
