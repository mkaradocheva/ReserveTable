namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;

    public class RestaurantServiceModel
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
    }
}
