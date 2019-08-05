namespace ReserveTable.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Domain;
    using ReserveTable.Services.Models;
    using ReserveTable.Mapping;

    public class RestaurantService : IRestaurantService
    {
        private readonly ReserveTableDbContext dbContext;

        public RestaurantService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateNewRestaurant(RestaurantServiceModel restaurantServiceModel)
        {
            Restaurant restaurant = AutoMapper.Mapper.Map<Restaurant>(restaurantServiceModel);

            await dbContext.Restaurants.AddAsync(restaurant);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfExistsInDb(RestaurantServiceModel restaurantServiceModel, string cityName)
        {
            var allRestaurants = await dbContext.Restaurants
                .Include(r => r.City)
                .ToListAsync();

            if (allRestaurants
                .Any(r => r.Name == restaurantServiceModel.Name
                && r.City.Name == cityName
                && r.Address == restaurantServiceModel.Address))
            {
                return true;
            }

            return false;
        }

        public async Task<RestaurantServiceModel> GetRestaurantByNameAndCity(string city, string name)
        {
            var restaurant = await dbContext
                .Restaurants
                .Include(r => r.Reviews)
                .Include(r => r.Tables)
                .ThenInclude(r => r.Reservations)
                .Where(r => r.Name == name && r.City.Name == city)
                .To<RestaurantServiceModel>()
                .FirstOrDefaultAsync();

            return restaurant;
        }

        public async Task<double> GetAverageRate(RestaurantServiceModel restaurantServiceModel)
        {
            var reviews = await dbContext.Reviews
                .Where(r => r.RestaurantId == restaurantServiceModel.Id)
                .ToListAsync();

            double average = Math.Round((reviews.Sum(r => r.Rate)) / reviews.Count(), 1);

            return average;
        }

        public async Task<bool> SetNewRating(string restaurantId, double rating)
        {
            var restaurant = dbContext.Restaurants.Find(restaurantId);
            restaurant.AverageRating = rating;
            dbContext.Restaurants.Update(restaurant);

            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<RestaurantServiceModel> GetRestaurantById(string id)
        {
            var restaurant = await dbContext.Restaurants.FindAsync(id);
            var restaurantServiceModel = AutoMapper.Mapper.Map<RestaurantServiceModel>(restaurant);

            return restaurantServiceModel;
        }
    }
}
