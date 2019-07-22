using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReserveTable.Domain;
using ReserveTable.Models.Reservations;
using ReserveTable.Services;

namespace ReserveTable.App.Controllers
{
    [Authorize]
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

        public IActionResult My()
        {
            var username = this.User.Identity.Name;
            var reservationsFromDb = reservationsService.GetMyReservations(username);

            var list = new List<MyReservationViewModel>();

            foreach (var reservation in reservationsFromDb)
            {
                var restaurant = restaurantService.GetRestaurantById(reservation.RestaurantId);

                var reservationViewModel = new MyReservationViewModel
                {
                    Date = reservation.ForDate.ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
                    Restaurant = restaurant.Name
                };

                list.Add(reservationViewModel);
            }

            MyReservationsListViewModel viewModel = new MyReservationsListViewModel
            {
                Reservations = list
            };

            return this.View(viewModel);
        }
    }
}