using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.App.Models.Cities;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Data;
using ReserveTable.Domain;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ReserveTableDbContext dbContext;

        //private readonly RestaurantService restaurantService;

        //public RestaurantsController(RestaurantService restaurantService)
        //{
        //    this.restaurantService = restaurantService;
        //}

        public RestaurantsController(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
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
            //TODO: Unique restaurants in database

            string cityId = dbContext.Cities
                .Where(c => c.Name == modelView.City)
                .Select(c => c.Id)
                .SingleOrDefault();

                var restaurant = new Restaurant
                {
                    Name = modelView.Name,
                    CityId = cityId,
                    Address = modelView.Address,
                    PhoneNumber = modelView.PhoneNumber
                };

                await dbContext.Restaurants.AddAsync(restaurant);
                await dbContext.SaveChangesAsync();

                return this.Redirect("/Home/Index");
        }

        public IActionResult All()
        {
            var allRestaurants = dbContext.Restaurants
                .Select(r => new AllRestaurantsViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    City = r.City.Name,
                })
                .ToList();

            return this.View(allRestaurants);
        }

        [HttpGet("/Restaurants/{city}/{restaurant}")]
        public IActionResult Details(string city, string restaurant)
        {
            var viewModel = dbContext.Restaurants
                .Where(r => r.City.Name == city
                        && r.Name == restaurant)
                .Select(r => new RestaurantDetailsViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address + ", " + r.City.Name,
                    PhoneNumber = r.PhoneNumber
                })
                .SingleOrDefault();


            return this.View(viewModel);
        }

    }
}