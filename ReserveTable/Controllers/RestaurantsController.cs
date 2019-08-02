namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Restaurants;
    using Domain;
    using ReserveTable.Models.Reviews;
    using Services;

    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICityService cityService;
        private readonly IUsersService usersService;

        public RestaurantsController(IRestaurantService restaurantService,
            ICityService cityService,
            IUsersService usersService)
        {
            this.restaurantService = restaurantService;
            this.cityService = cityService;
            this.usersService = usersService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var allCities = await cityService.GetAllCities();
            this.ViewData["cityNames"] = allCities;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRestaurantBindingModel modelView)
        {
            string cityId = await cityService.GetCityByName(modelView.City);

            var restaurant = new Restaurant
            {
                Name = modelView.Name,
                CityId = cityId,
                Address = modelView.Address,
                PhoneNumber = modelView.PhoneNumber
            };

            if (!await restaurantService.CheckIfExistsInDb(restaurant))
            {
                await restaurantService.CreateNewRestaurant(restaurant);
            }

            return this.Redirect("/Home/Index");
        }

        public async Task<IActionResult> All()
        {
            var allRestaurants = await restaurantService.GetAllRestaurants();

            return this.View(allRestaurants);
        }

        [HttpGet("/Restaurants/{city}/{restaurant}")]
        public async Task<IActionResult> Details(string city, string restaurant)
        {
            var restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var restaurantAverageRate = await restaurantService.GetAverageRate(restaurantFromDb);

            var reviewsViewModel = new List<AllReviewsForRestaurantViewModel>();

            foreach (var review in restaurantFromDb.Reviews)
            {
                var user = await usersService.GetUserById(review.UserId);

                reviewsViewModel.Add(new AllReviewsForRestaurantViewModel
                {
                    Username = user.UserName,
                    Comment = review.Comment,
                    Rate = review.Rate
                });
            }

            var restaurantViewModel = new RestaurantDetailsViewModel
            {
                Id = restaurantFromDb.Id,
                Name = restaurantFromDb.Name,
                Address = restaurantFromDb.Address,
                City = city,
                PhoneNumber = restaurantFromDb.PhoneNumber,
                AverageRate = restaurantAverageRate,
                Reviews = reviewsViewModel
            };

            return this.View(restaurantViewModel);
        }
    }
}