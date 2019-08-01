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
