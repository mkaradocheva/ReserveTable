namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Home;
    using ReserveTable.Services.Models;
    using Services;

    public class HomeController : Controller
    {
        private readonly ICityService cityService;

        public HomeController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<CityServiceModel> cities = await cityService.GetAllCities();
            List<CitiesHomeViewModel> viewModel = new List<CitiesHomeViewModel>();

            foreach (var city in cities)
            {
                var cityViewModel = AutoMapper.Mapper.Map<CitiesHomeViewModel>(city);
                viewModel.Add(cityViewModel);
            }

            return View(viewModel);
        }
    }
}
