using System;
using System.ComponentModel.DataAnnotations;

namespace ReserveTable.Domain
{
    public class Reservation
    {
        public string Id { get; set; }

        public DateTime DateMade => DateTime.Now;

        public DateTime ForDate { get; set; }

        public DateTime EndOfReservation => this.ForDate.AddHours(2);

        public string UserId { get; set; }
        public ReserveTableUser User { get; set; }

        [Range(1, 100)]
        public int SeatsCount { get; set; }

        public string TableId { get; set; }
        public Table Table { get; set; }
    }
}
