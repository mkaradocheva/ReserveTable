namespace ReserveTable.Models.Reservations
{
    using System.ComponentModel.DataAnnotations;

    public class CreateReservationBindingModel
    {
        private const string InvalidSeatsCountErrorMessage = "Seats must be a number between 1 and 99.";

        [Required]
        [Range(1, 99, ErrorMessage = InvalidSeatsCountErrorMessage)]
        public int SeatsCount { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public string Restaurant { get; set; }
    }
}
