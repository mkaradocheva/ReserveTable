namespace ReserveTable.App.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Reservations;
    using Services;

    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationsService reservationsService;
        private readonly IRestaurantService restaurantService;
        private readonly IUsersService usersService;

        public ReservationsController(IReservationsService reservationsService,
                IRestaurantService restaurantService,
                IUsersService usersService)
        {
            this.reservationsService = reservationsService;
            this.restaurantService = restaurantService;
            this.usersService = usersService;
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
            var restaurantFromDb = restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            var user = usersService.GetUserByUsername(this.User.Identity.Name);
            var reservation = reservationsService.MakeReservation(viewModel, user, restaurantFromDb);

            if (reservation == null)
            {
                return this.View();
            }
            else
            {
                return this.Redirect("/Reservations/My");
            }
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
                    Id = reservation.Id,
                    Date = reservation.ForDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Restaurant = restaurant.Name,
                    City = restaurant.City.Name
                };

                list.Add(reservationViewModel);
            }

            MyReservationsListViewModel viewModel = new MyReservationsListViewModel
            {
                Reservations = list
            };

            return this.View(viewModel);
        }

        [HttpGet("/Reservations/Cancel/{reservationId}")]
        public IActionResult Cancel(string reservationId)
        {
            var viewModel = reservationsService.GetReservationForCancel(reservationId);
            return this.View(viewModel);
        }

        [HttpPost("/Reservations/Cancel/{reservationId}")]
        public IActionResult CancelReservation(string reservationId)
        {
            reservationsService.CancelReservation(reservationId);

            return this.Redirect("/Reservations/My");
        }
    }
}