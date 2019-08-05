namespace ReserveTable.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ReserveTable.Models.Reservations;
    using ReserveTable.Services.Models;
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
                RestaurantServiceModel restaurantFromDb = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
                ReserveTableUserServiceModel user = await usersService.GetUserByUsername(this.User.Identity.Name);
                ReservationServiceModel reservation = await reservationsService.MakeReservation(viewModel, user, restaurantFromDb);

                if (reservation == null)
                {
                    ModelState.AddModelError("NoAvailableTables", "There are no available tables.");

                    return this.View();
                }

                return this.Redirect("/Reservations/My");
            }
            else
            {
                ModelState.AddModelError("InvalidData", "Your date is not valid.");
            }

            return this.View();
        }

        public async Task<IActionResult> My()
        {
            var username = this.User.Identity.Name;
            IQueryable<ReservationServiceModel> reservationsServiceModel = await reservationsService.GetMyReservations(username);

            var list = new List<MyReservationViewModel>();

            foreach (var reservation in reservationsServiceModel)
            {
                RestaurantServiceModel restaurant = await restaurantService.GetRestaurantById(reservation.RestaurantId);

                var reservationViewModel = AutoMapper.Mapper.Map<MyReservationViewModel>(reservation);

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
            var reservationServiceModel = await reservationsService.GetReservationById(reservationId);

            CancelReservationViewModel viewModel = AutoMapper.Mapper.Map<CancelReservationViewModel>(reservationServiceModel);

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