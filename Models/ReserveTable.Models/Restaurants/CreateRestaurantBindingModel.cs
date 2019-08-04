namespace ReserveTable.App.Models.Restaurants
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class CreateRestaurantBindingModel
    {
        private const string NameLengthErrorMessage = "Name of restaurant must be between 3 and 20 characters long.";
        private const string AddressLengthErrorMessage = "Address must be between 4 and 30 characters long.";

        [Required]
        [StringLength(20, ErrorMessage = NameLengthErrorMessage, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = AddressLengthErrorMessage)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
