using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherTest.WebUI.Models;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Sockets;

namespace WeatherTest.WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region Constants

        #endregion

        #region Constructors
        public HomeController(WeatherTestContext context)
        {
            _weatherTestDb = context;
        }

        #endregion

        #region Events + Delegates
        public delegate Task<ServiceResult> QueryServiceDelegate(WeatherService service, string location);

        #endregion

        #region Properties
        private WeatherTestContext _weatherTestDb = null;

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
        /// GET: /Home/CreateData
        /// </summary>
        public IActionResult CreateData()
        {
            DefaultData.CreateDemoData(WeatherTestDb);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: /Home/Error
        /// </summary>
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Evaluates the result of an expression
        /// </summary>
        /// <param name="expression">The calculation to perform</param>
        private decimal EvaluateExpression(string expression)
        {
            var dataTable = new DataTable();
            var column = new DataColumn("Evaluate", typeof(decimal), expression);
            dataTable.Columns.Add(column);
            dataTable.Rows.Add(0);
            return (decimal)(dataTable.Rows[0]["Evaluate"]);
        }

        /// <summary>
        /// GET: /Home/Index
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            using (HomeViewModel vm = new HomeViewModel())
            {
                vm.TemperatureUnits = new SelectList(WeatherTestDb.TemperatureUom.OrderBy(i => i.Title), "Id", "Title");
                vm.WindSpeedUnits = new SelectList(WeatherTestDb.WindSpeedUom.OrderBy(i => i.Title), "Id", "Title");

                return View(vm);
            }
        }

        /// <summary>
        /// POST: /Home/GetWeather
        /// </summary>
        /// <param name="vm">Submitted query</param>
        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel query)
        {
            query.TemperatureUnits = new SelectList(WeatherTestDb.TemperatureUom.OrderBy(i => i.Title), "Id", "Title");
            query.WindSpeedUnits = new SelectList(WeatherTestDb.WindSpeedUom.OrderBy(i => i.Title), "Id", "Title");

            if (!ModelState.IsValid)
                return View(query);
            
            query.Results = await QueryServices((int)query.SelectedTemperatureUnits, (int)query.SelectedWindSpeedUnits, query.Location);

            return View(query);
            
        }

        /// <summary>
        /// Queries the specified service
        /// </summary>
        /// <param name="service">The service to query</param>
        /// <param name="location">The location to search for</param>
        public async Task<ServiceResult> QueryService(WeatherService service, string location)
        {
            ServiceResult returnValue = new ServiceResult();

            string queryUri = string.Format(service.ServiceAddress.Replace("{location}", "{0}"), location);
            using (HttpClient httpClient = new HttpClient())
            {
                string jsonResult = await httpClient.GetStringAsync(queryUri);
                var jsonResultObject = JObject.Parse(jsonResult);

                returnValue.TemperatureResult = (decimal)jsonResultObject.SelectToken(service.TemperaturePath);
                returnValue.WindSpeedResult = (decimal)jsonResultObject.SelectToken(service.WindSpeedPath);
            }

            return returnValue;
        }

        /// <summary>
        /// Queries the configured services
        /// </summary>
        /// <param name="selectedTemperatureUnitId">Id of the selected temperature UOM</param>
        /// <param name="selectedWindSpeedUnitId">Id of the selected temperature UOM</param>
        /// <param name="location">The location being searched</param>
        /// <param name="queryServiceDelegate">Callback for querying a service</param>
        public async Task<QueryResults> QueryServices(int selectedTemperatureUnitId, int selectedWindSpeedUnitId, string location, QueryServiceDelegate queryServiceDelegate = null)
        {
            if (queryServiceDelegate == null)
                queryServiceDelegate = new QueryServiceDelegate(QueryService);

            QueryResults returnValue = new QueryResults()
            {
                ServicesOffline = 0,
                TotalServices = 0,
                TemperatureResult = 0,
                TemperatureUOM = WeatherTestDb.TemperatureUom.FirstOrDefault(i => i.Id == selectedTemperatureUnitId).Title,
                WindSpeedResult = 0,
                WindSpeedUOM = WeatherTestDb.WindSpeedUom.FirstOrDefault(i => i.Id == selectedWindSpeedUnitId).Title
            };

            IEnumerable<WeatherService> services = WeatherTestDb.WeatherService;
            returnValue.TotalServices = services.Count();

            List<decimal> temperatures = new List<decimal>();
            List<decimal> windSpeeds = new List<decimal>();

            foreach (WeatherService service in services)
            {
                try
                {
                    ServiceResult serviceResult = await queryServiceDelegate(service, location);
                    
                    if (selectedTemperatureUnitId != service.TemperatureUomid)
                    {
                        TemperatureConversion conversion = WeatherTestDb.TemperatureConversion.First(i =>
                            i.FromTemperatureUomid == service.TemperatureUomid &&
                            i.ToTemperatureUomid == selectedTemperatureUnitId);

                        string calculation = string.Format(conversion.Formula.Replace("{value}", "{0}"), serviceResult.TemperatureResult);
                        serviceResult.TemperatureResult = EvaluateExpression(calculation);
                    }
                    temperatures.Add(serviceResult.TemperatureResult);

                    if (selectedWindSpeedUnitId != service.WindSpeedUomid)
                    {
                        WindSpeedConversion conversion = WeatherTestDb.WindSpeedConversion.First(i =>
                            i.FromWindSpeedUomid == service.WindSpeedUomid &&
                            i.ToWindSpeedUomid == selectedWindSpeedUnitId);

                        string calculation = string.Format(conversion.Formula.Replace("{value}", "{0}"), serviceResult.WindSpeedResult);
                        serviceResult.WindSpeedResult = EvaluateExpression(calculation);
                    }
                    windSpeeds.Add(serviceResult.WindSpeedResult);
                }
                catch (HttpRequestException)
                {
                    returnValue.ServicesOffline++;
                }
            }

            if (temperatures.Count > 0)
                returnValue.TemperatureResult = temperatures.Average();
            if (windSpeeds.Count > 0)
                returnValue.WindSpeedResult = windSpeeds.Average();

            return returnValue;
        }

        #endregion

    }
}
