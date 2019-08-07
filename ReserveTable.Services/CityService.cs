namespace ReserveTable.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Models;
    using AutoMapper;
    using Mapping;
    using System;

    public class CityService : ICityService
    {
        private const string RatingAscendingCriteria = "rating-lowest-to-highest";
        private const string RatingDescendingCriteria = "rating-highest-to-lowest";
        private const string NameAscendingCriteria = "alphabetically-a-to-z";
        private const string NameDescendingCriteria = "alphabetically-z-to-a";
        private const int CityNameMinLength = 3;
        private const int CityNameMaxLength = 20;

        private readonly ReserveTableDbContext dbContext;

        public CityService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddCity(CityServiceModel cityServiceModel)
        {
            City city = Mapper.Map<City>(cityServiceModel);

            if (city.Name == string.Empty 
                || city.Name.Length < CityNameMinLength 
                || city.Name.Length > CityNameMaxLength)
            {
                throw new ArgumentException();
            }

            await dbContext.Cities.AddAsync(city);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfExists(CityServiceModel city)
        {
            if (await dbContext.Cities.AnyAsync(c => c.Name == city.Name))
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<string>> GetAllCitiesNames()
        {
            var citiesNames = await dbContext.Cities
                .Select(c => c.Name)
                .ToListAsync();

            return citiesNames;
        }

        public async Task<IQueryable<CityServiceModel>> GetAllCities()
        {
           var cities = dbContext.Cities.To<CityServiceModel>();

            return cities;
        }

        public async Task<string> GetCityByName(string cityName)
        {
            string cityId = await dbContext.Cities
                .Where(c => c.Name == cityName)
                .Select(c => c.Id)
                .SingleOrDefaultAsync();

            return cityId;
        }

        public async Task<IQueryable<RestaurantServiceModel>> GetRestaurantsInCity(string city, string criteria = null)
        {
            switch (criteria)
            {
                case RatingAscendingCriteria:
                    return this.GetAllRestaurantsInCityByRatingAscending(city).To<RestaurantServiceModel>();
                case RatingDescendingCriteria:
                    return this.GetAllRestaurantsInCityByRatingDescending(city).To<RestaurantServiceModel>();
                case NameAscendingCriteria:
                    return this.GetAllRestaurantsInCityByNameAscending(city).To<RestaurantServiceModel>();
                case NameDescendingCriteria:
                    return this.GetAllRestaurantsInCityByNameDescending(city).To<RestaurantServiceModel>();
            }

            var restaurants = dbContext.Restaurants
                .Include(r => r.City)
                .Where(r => r.City.Name == city)
                .To<RestaurantServiceModel>();

            return restaurants;
        }

        private IQueryable<Restaurant> GetAllRestaurantsInCityByRatingAscending(string city)
        {
            return this.dbContext.Restaurants
                            .Where(r => r.City.Name == city)
                            .OrderBy(r => r.AverageRating);
        }

        private IQueryable<Restaurant> GetAllRestaurantsInCityByRatingDescending(string city)
        {
            return this.dbContext.Restaurants
                            .Where(r => r.City.Name == city)
                            .OrderByDescending(r => r.AverageRating);
        }

        private IQueryable<Restaurant> GetAllRestaurantsInCityByNameAscending(string city)
        {
            return this.dbContext.Restaurants
                .Where(r => r.City.Name == city)
                .OrderBy(r => r.Name);
        }

        private IQueryable<Restaurant> GetAllRestaurantsInCityByNameDescending(string city)
        {
            return this.dbContext.Restaurants
                .Where(r => r.City.Name == city)
                .OrderByDescending(r => r.Name);
        }
    }
}
