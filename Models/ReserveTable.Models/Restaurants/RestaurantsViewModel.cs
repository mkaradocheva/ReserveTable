namespace ReserveTable.App.Models.Restaurants
{
    using AutoMapper;
    using ReserveTable.Mapping;
    using ReserveTable.Services.Models;

    public class RestaurantsViewModel : IMapFrom<RestaurantServiceModel>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Rate { get; set; }

        public string Photo { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<RestaurantServiceModel, RestaurantsViewModel>()
                .ForMember(dest => dest.Rate,
                opts => opts.MapFrom(origin => origin.AverageRating.ToString() != "0" ? origin.AverageRating.ToString() : "No ratings yet"));
        }
    }
}
