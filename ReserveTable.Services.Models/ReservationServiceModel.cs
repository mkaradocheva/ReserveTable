namespace ReserveTable.Services.Models
{
    using System;

    public class ReservationServiceModel
    {
        public string Id { get; set; }

        public DateTime DateMade => DateTime.Now;

        public DateTime ForDate { get; set; }

        public DateTime EndOfReservation => this.ForDate.AddHours(2);

        public string UserId { get; set; }
        public ReserveTableUserServiceModel User { get; set; }

        public int SeatsCount { get; set; }

        public bool IsCancelled { get; set; }

        public string TableId { get; set; }
        public TableServiceModel Table { get; set; }

        public string RestaurantId { get; set; }
        public RestaurantServiceModel Restaurant { get; set; }
    }
}
