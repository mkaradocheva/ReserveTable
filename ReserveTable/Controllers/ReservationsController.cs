namespace ReserveTable.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Reservations;
    using Services;

    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService reservationsService;
        private readonly IRestaurantService restaurantService;
        private readonly IUserService usersService;

        public ReservationsController(IReservationService reservationsService,
                IRestaurantService restaurantService,
                IUserService usersService)
        {
            this.reservationsService = reservationsService;
            this.restaurantService = restaurantService;
            this.usersService = usersService;
        }

        [Route("/Reservations/{city}/{restaurant}")]
        public async Task<IActionResult> Create(string city, string restaurant)
        {
            return View();
        }

        [HttpPost]
        [Route("/Reservations/{city}/{restaurant}")]
        public async Task<IActionResult> Create(string city, string restaurant, CreateReservationBindingModel viewModel)
        {
            var dateTime = DateTime.Parse(viewModel.Date + " " + viewModel.Time);
            var isDateValid = await reservationsService.IsDateValid(dateTime);

            if (isDateValid)
            {
                var restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
                var user = await usersService.GetUserByUsername(this.User.Identity.Name);
                var reservation = await reservationsService.MakeReservation(viewModel, user, restaurantFromDb);

                if (reservation == null)
                {
                    //TODO: Error handling

                    return this.View();
                }

                return this.Redirect("/Reservations/My");
            }
            else
            {
                //TODO: Error handling
            }

            return this.View();
        }

        public async Task<IActionResult> My()
        {
            var username = this.User.Identity.Name;
            var reservationsFromDb = await reservationsService.GetMyReservations(username);

            var list = new List<MyReservationViewModel>();

            foreach (var reservation in reservationsFromDb)
            {
                var restaurant = await restaurantService.GetRestaurantById(reservation.RestaurantId);

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
        public async Task<IActionResult> Cancel(string reservationId)
        {
            var viewModel = await reservationsService.GetReservationForCancel(reservationId);
            return this.View(viewModel);
        }

        [HttpPost("/Reservations/Cancel/{reservationId}")]
        public async Task<IActionResult> CancelReservation(string reservationId)
        {
            await reservationsService.CancelReservation(reservationId);

            return this.Redirect("/Reservations/My");
        }
    }
}