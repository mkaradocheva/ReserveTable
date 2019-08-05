namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using ReserveTable.Services.Models;

    public interface IRestaurantService
    {
        Task<bool> CreateNewRestaurant(RestaurantServiceModel restaurantServiceModel);

        Task<bool> CheckIfExistsInDb(RestaurantServiceModel restaurantServiceModel, string cityName);

        Task<RestaurantServiceModel> GetRestaurantByNameAndCity(string city, string name);

        Task<double> GetAverageRate(RestaurantServiceModel restaurantServiceModel);

        Task<bool> SetNewRating(string restaurantId, double rating);

        Task<RestaurantServiceModel> GetRestaurantById(string id);
    }
}
