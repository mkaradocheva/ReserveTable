namespace ReserveTable.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Domain;

    public class RestaurantService : IRestaurantService
    {
        private readonly ReserveTableDbContext dbContext;

        public RestaurantService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateNewRestaurant(Restaurant restaurant)
        {
            await dbContext.Restaurants.AddAsync(restaurant);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CheckIfExistsInDb(Restaurant restaurant, string cityName)
        {
            var allRestaurants = await dbContext.Restaurants
                .Include(r => r.City)
                .ToListAsync();

            if (allRestaurants
                .Any(r => r.Name == restaurant.Name
                && r.City.Name == cityName
                && r.Address == restaurant.Address))
            {
                return true;
            }

            return false;
        }

        public async Task<Restaurant> GetRestaurantByNameAndCity(string city, string name)
        {
            var restaurant = await dbContext
                .Restaurants
                .Include(r => r.Reviews)
                .Include(r => r.Tables)
                .ThenInclude(r => r.Reservations)
                .Where(r => r.Name == name && r.City.Name == city)
                .FirstOrDefaultAsync();

            return restaurant;
        }

        public async Task<double> GetAverageRate(Restaurant restaurant)
        {
            var reviews = await dbContext.Reviews
                .Where(r => r.RestaurantId == restaurant.Id)
                .ToListAsync();

            double average = Math.Round((reviews.Sum(r => r.Rate)) / reviews.Count(), 1);

            return average;
        }

        public async Task<bool> SetNewRating(Restaurant restaurant, double rating)
        {
            restaurant.AverageRating = rating;
            dbContext.Restaurants.Update(restaurant);

            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Restaurant> GetRestaurantById(string id)
        {
            var restaurant = await dbContext.Restaurants.FindAsync(id);

            return restaurant;
        }
    }
}
