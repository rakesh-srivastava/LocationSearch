using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjLocationSearch.Models
{
    public class TaskLocationSearch
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; } 
        public double timeToSearch { get; set; } //Overall Time Taken To Search
        public double timeToRead { get; set; } //Time to Return the Filtered Data
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string maxDistance { get; set; } //Restricting the Distance as per I/P
        public string maxResults { get; set; } //Restricting the No of records as per I/P
        public List<Location> AllLocations { get; set; } //List of filtered Locations as per Geographical Coordinates provided
        public string Error { get; set; }
        //public int FileRecords { get; set; }

        public void StartSearch(string pLatitude, string pLongitude, string pDistance, string pmaxResults)
        {
            startTime = DateTime.Now;
            Latitude = pLatitude;
            Longitude = pLongitude;
            maxDistance = pDistance;
            maxResults = pmaxResults;
        }

        public void EndSearch(string sError)
        {
            endTime = DateTime.Now;
            TimeSpan t = endTime - startTime;
            timeToSearch = t.TotalSeconds;
            Error = sError;
        }

    }
}
