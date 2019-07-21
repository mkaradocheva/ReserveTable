using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.App.Models.Cities;
using ReserveTable.App.Models.Restaurants;
using ReserveTable.Data;

namespace ReserveTable.App.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ReserveTableDbContext dbContext;

        public CitiesController(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("/Cities/{city}")]
        public IActionResult CityRestaurants(string city)
        {
            var restaurants = dbContext.Restaurants
                .Where(r => r.City.Name == city)
                .Select(r => new RestaurantsViewModel
                {
                    Name = r.Name
                })
                .ToList();

            var model = new CityRestaurantsViewModel
            {
                CityName = city,
                RestaurantsNames = restaurants
            };

            return View(model);
        }
    }
}