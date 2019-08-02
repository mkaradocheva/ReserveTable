namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class CityService : ICityService
    {
        private readonly ReserveTableDbContext dbContext;

        public CityService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddCity(City city)
        {
            await dbContext.Cities.AddAsync(city);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<string>> GetAllCities()
        {
            var citiesNames = await dbContext.Cities
                .Select(c => c.Name)
                .ToListAsync();

            return citiesNames;
        }

        public async Task<string> GetCityByName(string cityName)
        {
            string cityId = await dbContext.Cities
                .Where(c => c.Name == cityName)
                .Select(c => c.Id)
                .SingleOrDefaultAsync();

            return cityId;
        }

        public async Task<List<Restaurant>> GetRestaurantsInCity(string city)
        {
            var restaurants = await dbContext.Restaurants
                .Where(r => r.City.Name == city)
                .ToListAsync();

            return restaurants;
        }
    }
}
