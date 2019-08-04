namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cities;
    using Models.Restaurants;
    using Domain;
    using Services;
    using Microsoft.AspNetCore.Authorization;
    using ReserveTable.Models.Cities;

    public class CitiesController : Controller
    {
        private readonly ICityService cityService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IRestaurantService restaurantService;

        public CitiesController(ICityService cityService, 
            ICloudinaryService cloudinaryService,
            IRestaurantService restaurantService)
        {
            this.cityService = cityService;
            this.cloudinaryService = cloudinaryService;
            this.restaurantService = restaurantService;
        }

        [Authorize()]
        [HttpGet()]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCityBindingModel model)
        {
            string pictureUrl = await cloudinaryService.UploadPicture(model.Photo, model.Name, "city_images");

            var city = new City
            {
                Name = model.Name,
                Photo = pictureUrl
            };

            if (await cityService.CheckIfExists(city))
            {
                ModelState.AddModelError("CityExists", "This city already exists");

                return this.View();
            }

            await cityService.AddCity(city);

            return this.Redirect("/");
        }

        [Route("/Cities/{city}")]
        public async Task<IActionResult> CityRestaurants(string city)
        {
            List<Restaurant> restaurants = await cityService.GetRestaurantsInCity(city);

            List<RestaurantsViewModel> restaurantsViewModel = new List<RestaurantsViewModel>();

            foreach (var restaurant in restaurants)
            {
                var averageRate = await restaurantService.GetAverageRate(restaurant);

                restaurantsViewModel.Add(new RestaurantsViewModel
                {
                    Name = restaurant.Name,
                    Rate = averageRate.ToString() != "NaN" ? averageRate.ToString() : "No ratings yet",
                    Picture = restaurant.Photo
                });
            }

            var model = new CityRestaurantsViewModel
            {
                CityName = city,
                Restaurants = restaurantsViewModel
            };

            return this.View(model);
        }

    }
}