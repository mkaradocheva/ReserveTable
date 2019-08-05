namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ReserveTable.Services.Models;

    public interface ICityService
    {
        Task<string> GetCityByName(string cityName);

        Task<IQueryable<RestaurantServiceModel>> GetRestaurantsInCity(string city, string criteria = null);

        Task<bool> AddCity(CityServiceModel city);

        Task<IEnumerable<string>> GetAllCitiesNames();

        Task<IQueryable<CityServiceModel>> GetAllCities();

        Task<bool> CheckIfExists(CityServiceModel city);
    }
}
