namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using App.Models.Restaurants;
    using Domain;

    public interface IRestaurantService
    {
        Task<bool> CreateNewRestaurant(Restaurant restaurant);

        bool CheckIfExistsInDb(Restaurant restaurant);

        List<AllRestaurantsViewModel> GetAllRestaurants();

        Restaurant GetRestaurantByNameAndCity(string city, string name);

        double GetAverageRate(Restaurant restaurant);

        Restaurant GetRestaurantById(string id);
    }
}
