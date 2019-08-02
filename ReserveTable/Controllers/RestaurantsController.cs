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
    using System.Linq;

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
        public IActionResult Create()
        {
            var allCities = cityService.GetAllCities()
                .ToList();
            this.ViewData["cityNames"] = allCities;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRestaurantBindingModel modelView)
        {
            string cityId = cityService.GetCityByName(modelView.City);

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
            var restaurantAverageRate = restaurantService.GetAverageRate(restaurantFromDb);

            var reviewsViewModel = new List<AllReviewsForRestaurantViewModel>();
            foreach (var review in restaurantFromDb.Reviews)
            {
                reviewsViewModel.Add(new AllReviewsForRestaurantViewModel
                {
                    Username = usersService.GetUserById(review.UserId).UserName,
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