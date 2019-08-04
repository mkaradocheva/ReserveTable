namespace ReserveTable.App.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Domain;
    using ReserveTable.Models.Reviews;
    using Services;

    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewsService;
        private readonly IRestaurantService restaurantService;

        public ReviewsController(IReviewService reviewsService,
            IRestaurantService restaurantService)
        {
            this.reviewsService = reviewsService;
            this.restaurantService = restaurantService;
        }

        [Authorize]
        [HttpGet("/Reviews/Create/{city}/{restaurant}")]
        public async Task<IActionResult> Create(string city, string restaurant)
        {
            return View();
        }

        [Authorize]
        [HttpPost("/Reviews/Create/{city}/{restaurant}")]
        public async Task<IActionResult> Create(CreateReviewBindingModel model, string city, string restaurant)
        {
            var restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var review = new Review
            {
                Comment = model.Comment,
                Rate = model.Rate,
                Restaurant = restaurantFromDb,
                UserId = userId,
                Date = DateTime.Now
            };

            await reviewsService.CreateReview(review);

            return this.Redirect($"/Restaurants/{city}/{restaurant}");
        }
    }
}