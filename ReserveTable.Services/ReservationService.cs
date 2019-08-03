namespace ReserveTable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Domain;
    using Models.Reservations;
    using System.Threading.Tasks;

    public class ReservationService : IReservationService
    {
        private const string DateStringFormat = "dd/MM/yyyy HH:mm";

        private readonly ReserveTableDbContext dbContext;

        public ReservationService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Reservation> MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user, Restaurant restaurant)
        {
            var dateTime = DateTime.Parse(viewModel.Date + " " + viewModel.Time);

            var tablesWithExactCountSeats = restaurant.Tables
                .Where(t => t.SeatsCount == viewModel.SeatsCount)
                .ToList();

            var tablesWithSeatsCountPlusOne = restaurant.Tables
                .Where(t => t.SeatsCount == viewModel.SeatsCount + 1)
                .ToList();

            Reservation reservation = new Reservation();

            if (tablesWithExactCountSeats.Any())
            {
                foreach (var table in tablesWithExactCountSeats)
                {
                    if (table.Reservations.Any(t => (dateTime > t.ForDate && dateTime < t.EndOfReservation) && t.IsCancelled == false))
                    {
                        return null;
                    }
                    else
                    {
                        reservation.ForDate = dateTime;
                        reservation.SeatsCount = viewModel.SeatsCount;
                        reservation.UserId = user.Id;
                        reservation.Table = table;
                        reservation.Restaurant = restaurant;

                        await dbContext.Reservations.AddAsync(reservation);
                        await dbContext.SaveChangesAsync();

                        return reservation;
                    }
                }
            }
            else if (tablesWithSeatsCountPlusOne.Any())
            {
                foreach (var biggerTable in tablesWithSeatsCountPlusOne)
                {
                    if (biggerTable.Reservations.Any(t => (dateTime > t.ForDate && dateTime < t.EndOfReservation) && t.IsCancelled == false))
                    {
                        return null;
                    }
                    else
                    {
                        reservation.ForDate = dateTime;
                        reservation.SeatsCount = viewModel.SeatsCount;
                        reservation.UserId = user.Id;
                        reservation.Table = biggerTable;
                        reservation.Restaurant = restaurant;

                        await dbContext.Reservations.AddAsync(reservation);
                        await dbContext.SaveChangesAsync();

                        return reservation;
                    }
                }
            }

            return null;
        }

        public async Task<List<Reservation>> GetMyReservations(string username)
        {
            var reservations = await dbContext.Reservations
                .Where(r => r.User.UserName == username
                && r.ForDate.AddHours(2) > DateTime.Now
                && r.IsCancelled == false)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.City)
                .ToListAsync();

            return reservations;
        }

        public async Task<bool> CancelReservation(string reservationId)
        {
            Reservation reservation =  await dbContext.Reservations.FindAsync(reservationId);
            reservation.IsCancelled = true;

            dbContext.Reservations.Update(reservation);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<CancelReservationViewModel> GetReservationForCancel(string reservationId)
        {
            var reservation = await dbContext.Reservations
                .Where(r => r.Id == reservationId)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.City)
                .FirstOrDefaultAsync();

            var viewModel = new CancelReservationViewModel
            {
                Restaurant = reservation.Restaurant.Name,
                City = reservation.Restaurant.City.Name,
                Date = reservation.ForDate.ToString(DateStringFormat, CultureInfo.InvariantCulture)
            };

            return viewModel;
        }

        public async Task<bool> IsDateValid(DateTime dateForReservation)
        {
            if (dateForReservation > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}
