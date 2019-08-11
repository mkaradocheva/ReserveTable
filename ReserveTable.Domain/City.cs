namespace ReserveTable.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class City
    {
        private const int MaxNameLength = 20;

        public City()
        {
            this.Restaurants = new HashSet<Restaurant>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength, ErrorMessage = "Invalid city name. Name must not be longer than {0} characters.")]
        public string Name { get; set; }

        [Required]
        public string Photo { get; set; }

        public ICollection<Restaurant> Restaurants { get; set; }
    }
}
