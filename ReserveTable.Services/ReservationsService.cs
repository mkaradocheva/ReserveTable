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

    public class ReservationsService : IReservationsService
    {
        private const string DateStringFormat = "dd/MM/yyyy HH:mm";

        private readonly ReserveTableDbContext dbContext;

        public ReservationsService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Reservation> MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user, Restaurant restaurant)
        {
            string dateTime = viewModel.Date + " " + viewModel.Time;
            DateTime parsed = DateTime.Parse(dateTime);

            var tablesWithExactCountSeats = restaurant.Tables
                .Where(t => t.SeatsCount == viewModel.SeatsCount)
                .ToList();

            var tablesWithSeatsCountPlusOne = restaurant.Tables
                .Where(t => t.SeatsCount + 1 == viewModel.SeatsCount)
                .ToList();

            Reservation reservation = new Reservation();

            foreach (var table in tablesWithExactCountSeats)
            {
                if (table.Reservations.Any(t => (t.ForDate < parsed || t.EndOfReservation > parsed) && t.IsCancelled == false))
                {
                    return null;
                }
                else
                {
                    reservation.ForDate = parsed;
                    reservation.SeatsCount = viewModel.SeatsCount;
                    reservation.UserId = user.Id;
                    reservation.Table = table;
                    reservation.Restaurant = restaurant;

                    await dbContext.Reservations.AddAsync(reservation);
                    await dbContext.SaveChangesAsync();

                    return reservation;
                }
            }

            if (reservation == null)
            {
                foreach (var biggerTable in tablesWithSeatsCountPlusOne)
                {
                    if (biggerTable.Reservations.Any(t => (t.ForDate < parsed || t.EndOfReservation > parsed) && t.IsCancelled == false))
                    {
                        return null;
                    }
                    else
                    {
                        reservation.ForDate = parsed;
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
