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
        private readonly IUserService usersService;
        private readonly ICityService cityService;
        private readonly ICloudinaryService cloudinaryService;

        public RestaurantsController(IRestaurantService restaurantService,
            IUserService usersService,
            ICityService cityService,
            ICloudinaryService cloudinaryService)
        {
            this.restaurantService = restaurantService;
            this.usersService = usersService;
            this.cityService = cityService;
            this.cloudinaryService = cloudinaryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allCities = await cityService.GetAllCities();
            this.ViewData["cityNames"] = allCities;

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantBindingModel modelView)
        {
            string cityId = await cityService.GetCityByName(modelView.City);
            string pictureUrl = await cloudinaryService.UploadPicture(modelView.Photo, $"{modelView.City} {modelView.Name}", "restaurant_images");

            var restaurant = new Restaurant
            {
                Name = modelView.Name,
                CityId = cityId,
                Address = modelView.Address,
                PhoneNumber = modelView.PhoneNumber,
                Photo = pictureUrl
            };

            if (await restaurantService.CheckIfExistsInDb(restaurant, modelView.City))
            {
                TempData["RestaurantExists"] = "This restaurant already exists.";
            }
            else
            {
                await restaurantService.CreateNewRestaurant(restaurant);
            }

            return this.Redirect($"/Cities/{modelView.City}");
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
                AverageRate = restaurantAverageRate.ToString() != "NaN"
                            ? restaurantAverageRate.ToString()
                            : "No ratings yet",
                Reviews = reviewsViewModel
            };

            return this.View(restaurantViewModel);
        }
    }
}