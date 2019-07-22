using System.Collections.Generic;
using System.Linq;
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
                .Where(r => r.User.UserName == username)
                .ToList();

            return reservations;
        }
    }
}
