namespace ReserveTable.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Table
    {
        private const int MinSeatsCount = 1;
        private const int MaxSeatsCount = 15;

        public Table()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public string Id { get; set; }

        [Required]
        [Range(minimum: MinSeatsCount, maximum: MaxSeatsCount, ErrorMessage = "Seats count must be a number between {0} and {1}.")]
        public int SeatsCount { get; set; }

        [Required]
        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
