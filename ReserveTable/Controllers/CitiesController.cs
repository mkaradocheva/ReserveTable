namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cities;
    using Models.Restaurants;
    using Services;
    using Microsoft.AspNetCore.Authorization;
    using ReserveTable.Models.Cities;
    using ReserveTable.Services.Models;
    using System.Linq;

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

            CityServiceModel cityServiceModel = AutoMapper.Mapper.Map<CityServiceModel>(model);
            cityServiceModel.Photo = pictureUrl;

            if (await cityService.CheckIfExists(cityServiceModel))
            {
                ModelState.AddModelError("CityExists", "This city already exists");
                return this.View();
            }
            await cityService.AddCity(cityServiceModel);

            return this.Redirect("/");
        }

        [Route("/Cities/{city}")]
        public async Task<IActionResult> CityRestaurants(string city, [FromQuery]string criteria)
        {
            IQueryable<RestaurantServiceModel> restaurants = await cityService.GetRestaurantsInCity(city, criteria);

            List<RestaurantsViewModel> restaurantsViewModel = restaurants
                .Select(restaurant => AutoMapper.Mapper.Map<RestaurantsViewModel>(restaurant))
                .ToList();

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