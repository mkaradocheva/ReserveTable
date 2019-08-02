namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Domain;

    public class CityService : ICityService
    {
        private readonly ReserveTableDbContext dbContext;

        public CityService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AddCity(City city)
        {
            dbContext.Cities.Add(city);
            var result = dbContext.SaveChanges();

            return result > 0;
        }

        public IEnumerable<string> GetAllCities()
        {
            var citiesNames = dbContext.Cities
                .Select(c => c.Name)
                .ToList();

            return citiesNames;
        }

        public string GetCityByName(string cityName)
        {
            string cityId = dbContext.Cities
                .Where(c => c.Name == cityName)
                .Select(c => c.Id)
                .SingleOrDefault();

            return cityId;
        }

        public List<Restaurant> GetRestaurantsInCity(string city)
        {
            var restaurants = dbContext.Restaurants
                .Where(r => r.City.Name == city)
                .ToList();

            return restaurants;
        }
    }
}
