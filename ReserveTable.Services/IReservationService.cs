namespace ReserveTable.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ReserveTable.Models.Reservations;
    using Models;

    public interface IReservationService
    {
        Task<ReservationServiceModel> MakeReservation(CreateReservationBindingModel viewModel, ReserveTableUserServiceModel user, RestaurantServiceModel restaurant);

        Task<IQueryable<ReservationServiceModel>> GetMyReservations(string username);

        Task<bool> CancelReservation(string reservationId);

        Task<ReservationServiceModel> GetReservationById(string reservationId);

        Task<bool> IsDateValid(DateTime dateForReservation);
    }
}
