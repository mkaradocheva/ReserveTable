namespace ReserveTable.Models.Tables
{
    using System.ComponentModel.DataAnnotations;

    public class AddTableViewModel
    {
        private const int MinSeatsCount = 1;
        private const int MaxSeatsCount = 15;

        [Required]
        [Range(minimum: MinSeatsCount, maximum: MaxSeatsCount, ErrorMessage = "Seats count must be a number between {0} and {1}.")]
        public int SeatsCount { get; set; }
    }
}
