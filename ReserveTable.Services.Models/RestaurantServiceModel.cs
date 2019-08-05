namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;
    using AutoMapper;
    using ReserveTable.Domain;
    using ReserveTable.Mapping;

    public class RestaurantServiceModel : IMapFrom<Restaurant>, IMapTo<Restaurant>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public double AverageRating { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string CityId { get; set; }
        public CityServiceModel City { get; set; }

        public ICollection<TableServiceModel> Tables { get; set; }

        public ICollection<ReviewServiceModel> Reviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            //configuration
            //    .CreateMap<RestaurantServiceModel, RestaurantsViewModel>()
            //    .ForMember(destination => destination.AverageRating,)
            //    opts => opts.Map
        }
    }
}
