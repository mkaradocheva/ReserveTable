using System;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.Domain;
using ReserveTable.Models.Reservations;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationsService reservationsService;
        private readonly IRestaurantService restaurantService;

        public ReservationsController(IReservationsService reservationsService, IRestaurantService restaurantService)
        {
            this.reservationsService = reservationsService;
            this.restaurantService = restaurantService;
        }

        [Route("/Reservations/{city}/{restaurant}")]
        public IActionResult Create(string city, string restaurant)
        {
            return View();
        }

        [HttpPost]
        [Route("/Reservations/{city}/{restaurant}")]
        public IActionResult Create(string city, string restaurant, CreateReservationBindingModel viewModel)
        {
            string dateTime = viewModel.Date + " " + viewModel.Time;
            var parsedDateTime = DateTime.Parse(dateTime);

            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            restaurantService.CheckAvailability(parsedDateTime, restaurantFromDb);

            var user = (ReserveTableUser)this.User.Identity;
            reservationsService.MakeReservation(viewModel, user);

            return this.View();
        }
    }
}