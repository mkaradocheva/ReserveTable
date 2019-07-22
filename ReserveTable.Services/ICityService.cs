using System.Collections.Generic;
using ReserveTable.Domain;

namespace ReserveTable.Services
{
    public interface ICityService
    {
        string GetCityByName(string cityName);

        List<Restaurant> GetRestaurantsInCity(string city);
    }
}
