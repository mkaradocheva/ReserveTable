namespace ReserveTable.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;
    using Models.Reservations;

    public interface IReservationService
    {
        Task<Reservation> MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUser user, Restaurant restaurant);

        Task<List<Reservation>> GetMyReservations(string username);

        Task<bool> CancelReservation(string reservationId);

        Task<CancelReservationViewModel> GetReservationForCancel(string reservationId);

        Task<bool> IsDateValid(DateTime dateForReservation);
    }
}
