namespace ReserveTable.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Table
    {
        private const string SeatsCountErrorMessage = "Seats count must be a number between 1 and 15.";

        public Table()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public string Id { get; set; }

        [Required]
        [Range(1, 15, ErrorMessage = SeatsCountErrorMessage)]
        public int SeatsCount { get; set; }

        [Required]
        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
