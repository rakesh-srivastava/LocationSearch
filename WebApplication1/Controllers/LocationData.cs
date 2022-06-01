using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
namespace ProjLocationSearch.Controllers
{
    public class LocationData
    {
        //private readonly IConfiguration Configuration;
        static System.Runtime.Caching.MemoryCache memoryCache;
        static int refreshInterval = 10;

        //public LocationData(IConfiguration iConfig)
        //{
        //    Configuration = iConfig;
        //}
        #region getAllLocations

        public List<Location> getAllLocations()
        {
            try
            {
                List<Location> lLocations = new List<Location>();
                lLocations = getLocationsFromCache();

                return lLocations;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message.ToString());
                throw ex;
            }
        }
        private List<Location> getLocationsFromCache()
        {
            try
            {
                List<Location> cacheLocations = new List<Location>();

                //Check if the list of Locations is already in Cache
                if (memoryCache == null) memoryCache = new System.Runtime.Caching.MemoryCache("memoryCache");

                //If not makes the search
                if (!memoryCache.Contains("ListOfLocations"))
                {
                    //Datasource is in CSV
                    string fileName =AppDomain.CurrentDomain.BaseDirectory+"locations.csv";
                    cacheLocations = getLocationsFromFile(fileName);

                    //Add to the Cache
                    CacheItemPolicy policy = cachePolicy();
                    memoryCache.Set("ListOfLocations", cacheLocations, policy);
                }
                else cacheLocations = (List<Location>)memoryCache["ListOfLocations"];

                return cacheLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Location> getLocationsFromFile(string filename)
        {
            try
            {
                List<Location> fileLocations = new List<Location>();
                string[] csvInput = File.ReadAllLines(filename);
                //Implementing Parallelism Lambda expression for Performance Improvement
                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 4 };

                Parallel.For(0, csvInput.Length, parallelOptions, i =>
                {
                    try
                    {

                        string[] data = csvInput[i].Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string name = getLocationName(data);
                        double latitude = getLocationLatitude(data);
                        double longitude = getLocationLongitude(data);
                        
                        Location fileLocation = new Location(latitude, longitude,name);
                        lock (fileLocations)
                        {
                            fileLocations.Add(fileLocation);
                        }

                    }
                    catch { }
                });

                return fileLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
        private string getLocationName(string[] data)
        {
            return data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
        }

        private double getLocationLatitude(string[] data)
        {
            return data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
        }

        private double getLocationLongitude(string[] data)
        {
            return data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;
        }

        private CacheItemPolicy cachePolicy()
        {
            CacheItemPolicy cPolicy = new CacheItemPolicy
            {
                UpdateCallback = new CacheEntryUpdateCallback(CacheEntryUpdate),
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(refreshInterval)
            };

            return cPolicy;
        }
        private void CacheEntryUpdate(CacheEntryUpdateArguments args)
        {
            var cacheItem = memoryCache.GetCacheItem(args.Key);
            var cacheObj = cacheItem.Value;

            cacheItem.Value = cacheObj;
            args.UpdatedCacheItem = cacheItem;
            var policy = new CacheItemPolicy
            {
                UpdateCallback = new CacheEntryUpdateCallback(CacheEntryUpdate),
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(refreshInterval)
            };
            args.UpdatedCacheItemPolicy = policy;
        }

    }
}
