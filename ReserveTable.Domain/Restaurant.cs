namespace ReserveTable.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Restaurant
    {
        private const string NameErrorMessage = "Addres must be at max 20 characters long.";
        private const string AddressErrorMessage = "Address must be at max 30 characters long.";
        public const string AverageRateErrorMessage = "Rate must be a number between 1 and 10.";

        public Restaurant()
        {
            this.Tables = new HashSet<Table>();
            this.Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; }

        [Required]
        public string Photo { get; set; }

        public double AverageRating { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = AddressErrorMessage)]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string CityId { get; set; }
        public City City { get; set; }

        public ICollection<Table> Tables { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
