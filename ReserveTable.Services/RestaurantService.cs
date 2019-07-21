using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Data;
using ReserveTable.Domain;

namespace ReserveTable.Services
{
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
            if (dbContext.Restaurants.Any(r => r.Name == restaurant.Name && r.City.Name == restaurant.Name))
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
                .Where(r => r.Name == name && r.City.Name == city)
                .SingleOrDefault();

            return restaurant;
        }
    }
}
