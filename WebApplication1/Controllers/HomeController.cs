using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ProjLocationSearch.Models;
using Microsoft.Extensions.Configuration;

namespace ProjLocationSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       // private readonly IConfiguration Configuration;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public TaskLocationSearch findLocation(string Latitude, string Longitude, string maxDistance, string maxResults)
        {
            TaskLocationSearch tlList = new TaskLocationSearch();
            //Latitude = "52.2165425";
            //Longitude = "5.4778534";
            //maxDistance = "100";
            //maxResults = "50";
            try
            {
                TaskLocationSearch sr = new TaskLocationSearch();
                sr.StartSearch(Latitude, Longitude, maxDistance, maxResults);
                Location location = new Location(double.Parse(Latitude), double.Parse(Longitude),"Name");
                tlList = filterLocation(location, Int32.Parse(maxDistance), Int32.Parse(maxResults), sr);
                tlList.EndSearch("");
            }
            catch (Exception e)
            {
                ErrorLog.WriteLog(e.Message.ToString());
                tlList.EndSearch(e.Message.ToString());

            }
            //Return Json result as Output
            return tlList;
        }


        public TaskLocationSearch filterLocation(Location pLocation, int maxDistance, int maxResults, TaskLocationSearch tlsInput)
        {

            List<Location> filteredList = new List<Location>();
            //TaskLocationSearch srDetailed = sr;
            try
            {
                //Get all Locations
                List<Location> lLocations = new List<Location>();
                LocationData gData = new LocationData();
                DateTime t1 = DateTime.UtcNow;
                lLocations = gData.getAllLocations();
                DateTime t2 = DateTime.UtcNow;
                TimeSpan t = t2 - t1;
                double d = t.TotalSeconds;
                tlsInput.timeToRead = d;

                //Order By Distance
                //Parallelism to Improve the Search Performance
                List<Location> orderByDistance = lLocations.AsParallel().WithDegreeOfParallelism(4).OrderByDescending(o => o.CalculateDistance(pLocation)).ToList();

                //Filter the Locations as per I/P
                List<Location> filteredData = orderByDistance.AsParallel().WithDegreeOfParallelism(4).GroupBy(x => new { x.Distance, x.Longitude, x.Latitude })
                                                   .Select(g => g.First()).ToList().Where(x => x.Distance <= maxDistance).Take(maxResults).ToList();

                tlsInput.AllLocations = filteredData;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message.ToString());
                Location location = new Location(double.Parse("0"), double.Parse("0"),"Name");
                List<Location> errorList = new List<Location>();
                 tlsInput.AllLocations = errorList;
                return tlsInput;
            }
            return tlsInput;
        }
    }
}
