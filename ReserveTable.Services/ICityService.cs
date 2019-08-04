﻿namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;
    using ReserveTable.Models.Home;

    public interface ICityService
    {
        Task<string> GetCityByName(string cityName);

        Task<List<Restaurant>> GetRestaurantsInCity(string city, string criteria = null);

        Task<bool> AddCity(City city);

        Task<IEnumerable<string>> GetAllCities();

        Task<IEnumerable<CitiesHomeViewModel>> GetCitiesWithPicture();

        Task<bool> CheckIfExists(City city);
    }
}
