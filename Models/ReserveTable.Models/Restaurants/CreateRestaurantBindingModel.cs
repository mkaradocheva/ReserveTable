namespace ReserveTable.App.Models.Restaurants
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Mapping;
    using Services.Models;

    public class CreateRestaurantBindingModel : IMapTo<RestaurantServiceModel>, IHaveCustomMappings
    {
        private const int MinNameLength = 3;
        private const int MaxNameLength = 30;
        private const int MinAddressLength = 4;
        private const int MaxAddressLength = 30;

        [Required]
        [StringLength(maximumLength:MaxNameLength, ErrorMessage = "Name of restaurant must be between {0} and {1} characters long.", MinimumLength =  MinNameLength)]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [StringLength(maximumLength:MaxAddressLength, ErrorMessage = "Address must be between {0} and {1} characters long.", MinimumLength = MinAddressLength)]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<CreateRestaurantBindingModel, RestaurantServiceModel>()
                .ForMember(dest => dest.City,
                opt => opt.Ignore());
        }
    }
}
