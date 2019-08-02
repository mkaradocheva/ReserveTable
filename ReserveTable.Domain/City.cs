namespace ReserveTable.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class City
    {
        private const string InvalidCityNameErrorMessage = "Invalid city name. Name must not be longer than 20 characters.";

        public City()
        {
            this.Restaurants = new HashSet<Restaurant>();
        }

        public string Id { get; set; }

        [MaxLength(20, ErrorMessage = InvalidCityNameErrorMessage)]
        public string Name { get; set; }

        public string Photo { get; set; }

        public ICollection<Restaurant> Restaurants { get; set; }
    }
}
