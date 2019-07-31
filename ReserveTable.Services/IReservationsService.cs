using System.Collections.Generic;
using ReserveTable.Domain;
using ReserveTable.Models.Reservations;

namespace ReserveTable.Services
{
    public interface IReservationsService
    {
        Reservation MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user, Restaurant restaurant);

        List<Reservation> GetMyReservations(string username);

        void CancelReservation(string reservationId);

        CancelReservationViewModel GetReservationForCancel(string reservationId);
    }
}
