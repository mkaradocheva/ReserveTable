using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.Models.Reviews;

namespace ReserveTable.App.Controllers
{
    public class ReviewsController : Controller
    {
        [Route("/Reviews/Create/{city}/{restaurant}")]
        public IActionResult Create(string city, string restaurant)
        {
            return View();
        }

        [HttpPost]
        [Route("/Reviews/Create/{city}/{restaurant}")]
        public IActionResult Create(CreateReviewBindingModel model, string city, string restaurant)
        {

        }
    }
}