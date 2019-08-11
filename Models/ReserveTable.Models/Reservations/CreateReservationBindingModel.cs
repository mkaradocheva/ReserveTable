namespace ReserveTable.Models.Reservations
{
    using System.ComponentModel.DataAnnotations;

    public class CreateReservationBindingModel
    {
        private const int MinSeatsCount = 1;
        private const int MaxSeatsCount = 99;

        [Required]
        [Range(minimum: MinSeatsCount, maximum:MaxSeatsCount, ErrorMessage = "Seats must be a number between {0} and {1}.")]
        public int SeatsCount { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public string Restaurant { get; set; }
    }
}
