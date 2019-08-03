namespace ReserveTable.App.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("/Reviews/Create/{city}/{restaurant}")]
        [Authorize]
        public async Task<IActionResult> Create(string city, string restaurant)
        {
            return View();
        }

        [HttpPost("/Reviews/Create/{city}/{restaurant}")]
        [Authorize]
        public async Task<IActionResult> Create(CreateReviewBindingModel model, string city, string restaurant)
        {
            var restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await reviewsService.CreateReview(model, restaurantFromDb, userId);

            return this.Redirect($"/Restaurants/{city}/{restaurant}");
        }
    }
}