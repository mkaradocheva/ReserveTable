using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.Models.Reviews;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewsService reviewsService;
        private readonly IRestaurantService restaurantService;

        public ReviewsController(IReviewsService reviewsService,
            IRestaurantService restaurantService)
        {
            this.reviewsService = reviewsService;
            this.restaurantService = restaurantService;
        }

        [HttpGet("/Reviews/Create/{city}/{restaurant}")]
        public IActionResult Create(string city, string restaurant)
        {
            return View();
        }

        [HttpPost("/Reviews/Create/{city}/{restaurant}")]
        public IActionResult Create(CreateReviewBindingModel model, string city, string restaurant)
        {
            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            reviewsService.CreateReview(model, restaurantFromDb, userId);

            return this.Redirect($"/Restaurants/{city}/{restaurant}");
        }
    }
}