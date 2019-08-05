namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;
    using ReserveTable.Mapping;
    using ReserveTable.Domain;

    public class CityServiceModel : IMapFrom<City>, IMapTo<City>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public ICollection<RestaurantServiceModel> Restaurants { get; set; }
    }
}
