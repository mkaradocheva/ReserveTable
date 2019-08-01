namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using Domain;

    public interface ICityService
    {
        string GetCityByName(string cityName);

        List<Restaurant> GetRestaurantsInCity(string city);
    }
}
