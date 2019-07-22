﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Domain;
using ReserveTable.Models.Reviews;
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
            var restaurantAverageRate = restaurantService.GetAverageRate(restaurantFromDb);

            var reviewsViewModel = new List<AllReviewsForRestaurantViewModel>();
            foreach (var review in restaurantFromDb.Reviews)
            {
                reviewsViewModel.Add(new AllReviewsForRestaurantViewModel
                {
                    Username = review.User.UserName,
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