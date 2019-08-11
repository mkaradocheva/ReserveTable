namespace ReserveTable.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Restaurant
    {
        private const int MaxNameLength = 30;
        private const int MaxAddressLength = 30;

        public Restaurant()
        {
            this.Tables = new HashSet<Table>();
            this.Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength, ErrorMessage = "Name must be at max {0} characters long.")]
        public string Name { get; set; }

        [Required]
        public string Photo { get; set; }

        public double AverageRating { get; set; }

        [Required]
        [MaxLength(MaxAddressLength, ErrorMessage = "Address must be at max {0} characters long.")]
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
