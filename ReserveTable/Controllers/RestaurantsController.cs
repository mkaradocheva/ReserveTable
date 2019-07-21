using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Domain;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICityService cityService;

        public RestaurantsController(IRestaurantService restaurantService, ICityService cityService)
        {
            this.restaurantService = restaurantService;
            this.cityService = cityService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRestaurantModelView modelView)
        {
            string cityId = cityService.FindCityByName(modelView.City);

            var restaurant = new Restaurant
            {
                Name = modelView.Name,
                CityId = cityId,
                Address = modelView.Address,
                PhoneNumber = modelView.PhoneNumber
            };

            if (!restaurantService.CheckIfExistsInDb(restaurant))
            {
                await restaurantService.CreateNewRestaurant(restaurant);
            }

            return this.Redirect("/Home/Index");
        }

        public IActionResult All()
        {
            var allRestaurants = restaurantService.GetAllRestaurants();

            return this.View(allRestaurants);
        }

        [HttpGet("/Restaurants/{city}/{restaurant}")]
        public IActionResult Details(string city, string restaurant)
        {
            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);

            var viewModel = new RestaurantDetailsViewModel
            {
                Id = restaurantFromDb.Id,
                Name = restaurantFromDb.Name,
                Address = restaurantFromDb.Address + ", " + city,
                PhoneNumber = restaurantFromDb.PhoneNumber
            };

            return this.View(viewModel);
        }
    }
}