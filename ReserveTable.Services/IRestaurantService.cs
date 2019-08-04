namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Domain;

    public interface IRestaurantService
    {
        Task<bool> CreateNewRestaurant(Restaurant restaurant);

        Task<bool> CheckIfExistsInDb(Restaurant restaurant, string cityName);

        Task<Restaurant> GetRestaurantByNameAndCity(string city, string name);

        Task<double> GetAverageRate(Restaurant restaurant);

        Task<bool> SetNewRating(Restaurant restaurant, double rating);

        Task<Restaurant> GetRestaurantById(string id);
    }
}
