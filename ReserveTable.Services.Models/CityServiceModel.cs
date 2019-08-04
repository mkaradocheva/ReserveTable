namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;

    public class CityServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public ICollection<RestaurantServiceModel> Restaurants { get; set; }
    }
}
