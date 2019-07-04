using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
                };

                await dbContext.Restaurants.AddAsync(restaurant);
                await dbContext.SaveChangesAsync();

                return this.Redirect("/Home/Index");
        }
    }
}