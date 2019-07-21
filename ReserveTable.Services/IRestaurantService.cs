using System.Collections.Generic;
using System.Threading.Tasks;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Domain;

namespace ReserveTable.Services
{
    public interface IRestaurantService
    {
        Task CreateNewRestaurant(Restaurant restaurant);

        bool CheckIfExistsInDb(Restaurant restaurant);

        List<AllRestaurantsViewModel> GetAllRestaurants();

        Restaurant GetRestaurantByNameAndCity(string city, string name);
    }
}
