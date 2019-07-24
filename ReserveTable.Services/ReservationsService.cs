using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReserveTable.Data;
using ReserveTable.Domain;
using ReserveTable.Models.Reservations;

namespace ReserveTable.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly ReserveTableDbContext dbContext;

        public ReservationsService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user)
        {
            var reservation = new Reservation
            {
                //ForDate = DateTime.Parse(dateTime),
                SeatsCount = viewModel.SeatsCount,
                UserId = user.Id,
            };
        }

        public List<Reservation> GetMyReservations(string username)
        {
            var reservations = dbContext.Reservations
                .Where(r => r.User.UserName == username 
                && r.ForDate.AddHours(2) > DateTime.Now
                && r.IsCancelled == false)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.City)
                .ToList();

            return reservations;
        }

        public void CancelReservation(string reservationId)
        {
            Reservation reservation = dbContext.Reservations.Find(reservationId);
            reservation.IsCancelled = true;

            dbContext.Reservations.Update(reservation);
            dbContext.SaveChanges();
        }

        public CancelReservationViewModel GetReservationForCancel(string reservationId)
        {
            var reservation = dbContext.Reservations
                .Where(r => r.Id == reservationId)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.City)
                .FirstOrDefault();

            var viewModel = new CancelReservationViewModel
            {
                Restaurant = reservation.Restaurant.Name,
                City = reservation.Restaurant.City.Name,
                Date = reservation.ForDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
            };

            return viewModel;
        }
    }
}
