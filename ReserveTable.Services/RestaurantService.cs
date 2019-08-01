namespace ReserveTable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using App.Models.Restaurants;
    using Data;
    using Domain;

    public class RestaurantService : IRestaurantService
    {
        private readonly ReserveTableDbContext dbContext;

        public RestaurantService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateNewRestaurant(Restaurant restaurant)
        {
            await dbContext.Restaurants.AddAsync(restaurant);
            await dbContext.SaveChangesAsync();
        }

        public bool CheckIfExistsInDb(Restaurant restaurant)
        {
            if (dbContext.Restaurants
                .Any(r => r.Name == restaurant.Name 
                && r.City.Name == restaurant.Name))
            {
                return true;
            }

            return false;
        }

        public List<AllRestaurantsViewModel> GetAllRestaurants()
        {
            var allRestaurants = dbContext.Restaurants
                .Select(r => new AllRestaurantsViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    City = r.City.Name,
                })
                .ToList();

            return allRestaurants;
        }

        public Restaurant GetRestaurantByNameAndCity(string city, string name)
        {
            var restaurant = dbContext
                .Restaurants
                .Include(r => r.Reviews)
                .Include(r => r.Tables)
                .ThenInclude(r => r.Reservations)
                .Where(r => r.Name == name && r.City.Name == city)
                .FirstOrDefault();

            return restaurant;
        }

        public double GetAverageRate(Restaurant restaurant)
        {
            var reviews = dbContext.Reviews
                .Where(r => r.RestaurantId == restaurant.Id);

            double average = Math.Round((reviews.Sum(r => r.Rate)) / reviews.Count(), 1);

            return average;
        }

        public Restaurant GetRestaurantById(string id)
        {
            var restaurant = dbContext.Restaurants.Find(id);

            return restaurant;
        }
    }
}
