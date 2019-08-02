namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using Domain;
    using Models.Reservations;

    public interface IReservationsService
    {
        Reservation MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user, Restaurant restaurant);

        List<Reservation> GetMyReservations(string username);

        bool CancelReservation(string reservationId);

        CancelReservationViewModel GetReservationForCancel(string reservationId);
    }
}
