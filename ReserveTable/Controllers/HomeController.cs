using Microsoft.AspNetCore.Mvc;

namespace ReserveTable.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
