using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using ProjLocationSearch;
using ProjLocationSearch.Controllers;
using ProjLocationSearch.Models;
using Xunit.Sdk;

namespace LocSearchUnitTest
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
            TaskLocationSearch sr = new TaskLocationSearch();
            TaskLocationSearch actual;
            //Act
            sr.StartSearch("52.2165425", "5.4778534", maxDistance.ToString(), maxResults.ToString());
            actual = sv.filterLocation(pLocation, maxDistance, maxResults, sr);
            //Assert
            Assert.AreEqual(maxResults.ToString(), actual.maxResults);
        }
    }
}
