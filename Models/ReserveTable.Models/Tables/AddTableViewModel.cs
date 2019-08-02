namespace ReserveTable.Models.Tables
{
    using System.ComponentModel.DataAnnotations;

    public class AddTableViewModel
    {
        private const string SeatsCountErrorMessage = "Seats count must be a number between 1 and 15.";

        [Required]
        [Range(1, 15, ErrorMessage = SeatsCountErrorMessage)]
        public int SeatsCount { get; set; }
    }
}
