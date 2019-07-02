﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReserveTable.Domain
{
    public class Restaurant
    {
        private const string InvalidRestaurantNameErrorMessage = "Invalid restaurant name. Name must not be longer than 20 characters.";

        public Restaurant()
        {
            this.Tables = new HashSet<Table>();
        }

        public string Id { get; set; }

        [MaxLength(20, ErrorMessage = InvalidRestaurantNameErrorMessage)]
        public string Name { get; set; }

        public bool HasAvailableTables { get; set; }

        public string CityId { get; set; }
        public City City { get; set; }

        public ICollection<Table> Tables { get; set; }
    }
}
