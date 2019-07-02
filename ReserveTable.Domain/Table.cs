using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReserveTable.Domain
{
    public class Table
    {
        public Table()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public string Id { get; set; }

        [Range(1, 10)]
        public int SeatsCount { get; set; }

        public string RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
