namespace ReserveTable.App.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Tables;
    using Services;

    [Authorize(Roles = "Admin")]
    public class TablesController : Controller
    {
        private readonly ITableService tablesService;
        private readonly IRestaurantService restaurantService;

        public TablesController(ITableService tablesService,
                    IRestaurantService restaurantService)
        {
            this.tablesService = tablesService;
            this.restaurantService = restaurantService;
        }

        [HttpGet("/Tables/{city}/{restaurant}")]
        public async Task<IActionResult> RestaurantTables(string city, string restaurant)
        {
            var restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var restaurantTables = await tablesService.GetRestaurantTables(restaurantFromDb);

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
        public async Task<IActionResult> Add(string city, string restaurant)
        {
            return this.View();
        }

        [HttpPost("/Tables/{city}/{restaurant}/Add")]
        public async Task<IActionResult> Add(string city, string restaurant, AddTableBindingModel model)
        {
            var restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            await tablesService.AddTable(model, restaurantFromDb);

            return this.Redirect($"/Tables/{city}/{restaurant}");
        }
    }
}