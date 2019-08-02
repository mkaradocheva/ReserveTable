namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface ICityService
    {
        Task<string> GetCityByName(string cityName);

        Task<List<Restaurant>> GetRestaurantsInCity(string city);

        Task<bool> AddCity(City city);

        Task<IEnumerable<string>> GetAllCities();
    }
}
