namespace ReserveTable.Models.Reservations
{
    public class CreateReservationBindingModel
    {
        public int SeatsCount { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Restaurant { get; set; }
    }
}
