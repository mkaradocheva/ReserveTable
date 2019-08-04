namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;

    public class TableServiceModel
    {
        public string Id { get; set; }

        public int SeatsCount { get; set; }

        public string RestaurantId { get; set; }
        public RestaurantServiceModel Restaurant { get; set; }

        public ICollection<ReservationServiceModel> Reservations { get; set; }
    }
}
