namespace ReserveTable.Models.Cities
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using Mapping;
    using Services.Models;

    public class CreateCityBindingModel : IMapTo<CityServiceModel>
    {
        private const int MinNameLength = 3;
        private const int MaxNameLength = 20;

        [Required]
        [StringLength(MaxNameLength, ErrorMessage = "City name must be at least {2} and at max {1} characters long.", MinimumLength = MinNameLength)]
        public string Name { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
