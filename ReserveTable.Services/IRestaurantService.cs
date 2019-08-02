namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using App.Models.Restaurants;
    using Domain;

    public interface IRestaurantService
    {
        Task<bool> CreateNewRestaurant(Restaurant restaurant);

        Task<bool> CheckIfExistsInDb(Restaurant restaurant);

        Task<List<AllRestaurantsViewModel>> GetAllRestaurants();

        Task<Restaurant> GetRestaurantByNameAndCity(string city, string name);

        Task<double> GetAverageRate(Restaurant restaurant);

        Task<Restaurant> GetRestaurantById(string id);
    }
}
