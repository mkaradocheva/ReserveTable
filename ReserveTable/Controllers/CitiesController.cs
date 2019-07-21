using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.App.Models.Cities;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Domain;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ICityService cityService;

        public CitiesController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        [Route("/Cities/{city}")]
        public IActionResult CityRestaurants(string city)
        {
            List<Restaurant> restaurants = cityService.GetRestaurantsInCity(city);

            List<RestaurantsViewModel> restaurantsViewModel = new List<RestaurantsViewModel>();

            foreach (var restaurant in restaurants)
            {
                restaurantsViewModel.Add(new RestaurantsViewModel
                {
                    Name = restaurant.Name
                });
            }

            var model = new CityRestaurantsViewModel
            {
                CityName = city,
                RestaurantsNames = restaurantsViewModel
            };

            return View(model);
        }
    }
}