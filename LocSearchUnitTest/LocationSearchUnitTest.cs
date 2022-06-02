using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ProjLocationSearch;
using ProjLocationSearch.Controllers;
using ProjLocationSearch.Models;
using Xunit.Sdk;

namespace ProjLocationSearchUnitTest
{
    [TestClass]
    public class LocationSearchUnitTest
    {
        [TestMethod]
        public void FetchLocationTest()
        {
            //Arrange
            HomeController sv = new HomeController();
            Location pLocation = new Location(double.Parse("51.644054"), double.Parse("5.6548692"),"testname");
            int maxDistance = 100;
            int maxResults = 10;
            TaskLocationSearch taskLocationSearch = new TaskLocationSearch();
            TaskLocationSearch actualLocation;
            //Act
            taskLocationSearch.StartSearch("52.2165425", "5.4778534", maxDistance.ToString(), maxResults.ToString());
            actualLocation = sv.filterLocation(pLocation, maxDistance, maxResults, taskLocationSearch);
            
            Assert.AreEqual(maxResults.ToString(), actualLocation.maxResults);
        }
    }
}
