namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Home;
    using ReserveTable.Services;

    public class HomeController : Controller
    {
        private readonly ICityService cityService;

        public HomeController(ICityService cityService)
        {
            this.cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
             IEnumerable<CitiesHomeViewModel> citiesViewModel = await cityService.GetCitiesWithPicture();

            return View(citiesViewModel);
        }
    }
}
