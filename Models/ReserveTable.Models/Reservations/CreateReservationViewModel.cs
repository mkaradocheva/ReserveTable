using System;

namespace ReserveTable.Models.Reservations
{
    public class CreateReservationViewModel
    {
        public int SeatsCount { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }
    }
}
