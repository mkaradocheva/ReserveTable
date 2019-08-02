namespace ReserveTable.Models.Cities
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class CreateCityBindingModel
    {
        private const string NameLengthErrorMessage = "City name must be between 3 and 20 characters long.";

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = NameLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
