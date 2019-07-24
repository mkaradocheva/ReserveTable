using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.Models.Tables;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TablesController : Controller
    {
        private readonly ITablesService tablesService;
        private readonly IRestaurantService restaurantService;

        public TablesController(ITablesService tablesService,
                    IRestaurantService restaurantService)
        {
            this.tablesService = tablesService;
            this.restaurantService = restaurantService;
        }

        [HttpGet("/Tables/{city}/{restaurant}")]
        public IActionResult RestaurantTables(string city, string restaurant)
        {
            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var restaurantTables = tablesService.GetRestaurantTables(restaurantFromDb);

            var tablesList = new List<RestaurantTablesViewModel>();

            foreach (var table in restaurantTables)
            {
                var tableViewModel = new RestaurantTablesViewModel
                {
                    SeatsCount = table.SeatsCount,
                    Id = table.Id
                };

                tablesList.Add(tableViewModel);
            }

            var viewModelList = new RestaurantTablesListViewModel
            {
                Tables = tablesList,
                RestaurantName = restaurant,
                CityName = city
            };

            return View(viewModelList);
        }

        [HttpGet("/Tables/{city}/{restaurant}/Add")]
        public IActionResult Add(string city, string restaurant)
        {
            return this.View();
        }

        [HttpPost("/Tables/{city}/{restaurant}/Add")]
        public IActionResult Add(string city, string restaurant, AddTableBindingModel model)
        {
            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            tablesService.AddTable(model, restaurantFromDb);

            return this.Redirect($"/Tables/{city}/{restaurant}");
        }

    }
}