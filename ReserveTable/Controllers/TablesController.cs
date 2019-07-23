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
        [Authorize(Roles = "Admin")]
        public IActionResult RestaurantTables(string city, string restaurant)
        {
            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var restaurantTables = tablesService.GetRestaurantTables(restaurantFromDb);

            var tablesList = new List<RestaurantTablesViewModel>();

            foreach (var table in restaurantTables)
            {
                var tableViewModel = new RestaurantTablesViewModel
                {
                    SeatsCount = table.SeatsCount
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
    }
}