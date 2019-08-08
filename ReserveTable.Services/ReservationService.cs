namespace ReserveTable.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Domain;
    using ReserveTable.Models.Reservations;
    using Models;
    using Mapping;

    public class ReservationService : IReservationService
    {

        private readonly ReserveTableDbContext dbContext;

        public ReservationService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ReservationServiceModel> MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUserServiceModel user, RestaurantServiceModel restaurant)
        {
            var dateTime = DateTime.Parse(viewModel.Date + " " + viewModel.Time);

            var tablesWithExactCountSeats = restaurant.Tables
                .Where(t => t.SeatsCount == viewModel.SeatsCount)
                .ToList();

            var tablesWithSeatsCountPlusOne = restaurant.Tables
                .Where(t => t.SeatsCount == viewModel.SeatsCount + 1)
                .ToList();

            ReservationServiceModel reservationServiceModel = new ReservationServiceModel();

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
                        reservationServiceModel.ForDate = dateTime;
                        reservationServiceModel.SeatsCount = viewModel.SeatsCount;
                        reservationServiceModel.UserId = user.Id;
                        reservationServiceModel.TableId = table.Id;
                        reservationServiceModel.RestaurantId = restaurant.Id;

                        var reservation = AutoMapper.Mapper.Map<Reservation>(reservationServiceModel);

                        await dbContext.Reservations.AddAsync(reservation);
                        await dbContext.SaveChangesAsync();

                        return reservationServiceModel;
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
                        reservationServiceModel.ForDate = dateTime;
                        reservationServiceModel.SeatsCount = viewModel.SeatsCount;
                        reservationServiceModel.UserId = user.Id;
                        reservationServiceModel.Table = biggerTable;
                        reservationServiceModel.Restaurant = restaurant;

                        var reservation = AutoMapper.Mapper.Map<Reservation>(reservationServiceModel);

                        await dbContext.Reservations.AddAsync(reservation);
                        await dbContext.SaveChangesAsync();

                        return reservationServiceModel;
                    }
                }
            }

            return null;
        }

        public async Task<IQueryable<ReservationServiceModel>> GetMyReservations(string username)
        {
            var reservationsServiceModel = dbContext.Reservations
                .Where(r => r.User.UserName == username
                && r.ForDate.AddHours(2) > DateTime.Now
                && r.IsCancelled == false)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.City)
                .To<ReservationServiceModel>();

            return reservationsServiceModel;
        }

        public async Task<bool> CancelReservation(string reservationId)
        {
            var reservation = dbContext.Reservations.Find(reservationId);

            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            reservation.IsCancelled = true;
            dbContext.Reservations.Update(reservation);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ReservationServiceModel> GetReservationById(string reservationId)
        {
            var reservation = await dbContext.Reservations
                .Where(r => r.Id == reservationId)
                .Include(r => r.Restaurant)
                .ThenInclude(r => r.City)
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                throw new ArgumentNullException(nameof(reservation));
            }

            var reservationServiceModel = AutoMapper.Mapper.Map<ReservationServiceModel>(reservation);

            return reservationServiceModel;
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
