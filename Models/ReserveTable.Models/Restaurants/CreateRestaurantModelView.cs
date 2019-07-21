using System.ComponentModel.DataAnnotations;

namespace ReserveTable.App.Models.Restaurants
{
    public class CreateRestaurantModelView
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
