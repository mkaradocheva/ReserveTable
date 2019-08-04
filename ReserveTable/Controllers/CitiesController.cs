﻿namespace ReserveTable.App.Controllers
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

        public CitiesController(ICityService cityService, 
            ICloudinaryService cloudinaryService)
        {
            this.cityService = cityService;
            this.cloudinaryService = cloudinaryService;
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
        public async Task<IActionResult> CityRestaurants(string city, [FromQuery]string criteria)
        {
            List<Restaurant> restaurants = await cityService.GetRestaurantsInCity(city, criteria);

            List<RestaurantsViewModel> restaurantsViewModel = new List<RestaurantsViewModel>();

            foreach (var restaurant in restaurants)
            {
                restaurantsViewModel.Add(new RestaurantsViewModel
                {
                    Name = restaurant.Name,
                    Rate = restaurant.AverageRating.ToString() != "0" ? restaurant.AverageRating.ToString() : "No ratings yet",
                    Picture = restaurant.Photo
                });
            }

            var model = new CityRestaurantsViewModel
            {
                CityName = city,
                Restaurants = restaurantsViewModel
            };

            this.ViewData["criteria"] = criteria;

            return this.View(model);
        }

    }
}