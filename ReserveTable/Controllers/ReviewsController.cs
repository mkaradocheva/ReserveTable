namespace ReserveTable.App.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Reviews;
    using ReserveTable.Services.Models;
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
            var restaurantFromDbServiceModel = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ReviewServiceModel reviewServiceModel = AutoMapper.Mapper.Map<ReviewServiceModel>(model);
            reviewServiceModel.RestaurantId = restaurantFromDbServiceModel.Id;
            reviewServiceModel.UserId = userId;
            reviewServiceModel.Date = DateTime.Now;

            await reviewsService.CreateReview(reviewServiceModel);
            var newRestaurantAverageRating = await restaurantService.GetAverageRate(restaurantFromDbServiceModel);
            await restaurantService.SetNewRating(restaurantFromDbServiceModel.Id, newRestaurantAverageRating);

            return this.Redirect($"/Restaurants/{city}/{restaurant}");
        }
    }
}