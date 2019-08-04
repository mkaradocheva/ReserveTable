namespace ReserveTable.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reservation
    {
        public string Id { get; set; }

        public DateTime DateMade => DateTime.Now;

        [Required]
        public DateTime ForDate { get; set; }

        public DateTime EndOfReservation => this.ForDate.AddHours(2);

        [Required]
        public string UserId { get; set; }
        public ReserveTableUser User { get; set; }

        [Required]
        [Range(1, 99)]
        public int SeatsCount { get; set; }

        public bool IsCancelled { get; set; }

        public string TableId { get; set; }
        public Table Table { get; set; }

        [Required]
        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
