using System.Collections.Generic;
using ReserveTable.Domain;
using ReserveTable.Models.Reservations;

namespace ReserveTable.Services
{
    public interface IReservationsService
    {
        void MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user);

        List<Reservation> GetMyReservations(string username);
    }
}
