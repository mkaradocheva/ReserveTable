using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.Data;
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
    }
}