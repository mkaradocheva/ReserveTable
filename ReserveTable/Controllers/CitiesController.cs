namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cities;
    using Models.Restaurants;
    using Domain;
    using Services;

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