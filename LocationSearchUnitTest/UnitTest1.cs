using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebApplication1;



namespace WebApplication1.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        //Unit Test case to Get Location
        public void FetchLocationTest()
        {
            //Arrange
            Service1 sv = new Service1();
            Location pLocation = new Location(double.Parse("51.644054"), double.Parse("5.6548692"));
            int maxDistance = 100;
            int maxResults = 10;
            Search sr = new Search();
            Search actual;
            //Act
            sr.StartSearch("52.2165425", "5.4778534", maxDistance.ToString(), maxResults.ToString());
            actual = sv.FetchLocOnFilter(pLocation, maxDistance, maxResults, sr);
            //Assert
            Assert.AreEqual(maxResults.ToString(), actual.maxResults);
        }
    }
}
