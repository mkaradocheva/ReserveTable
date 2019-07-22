using System.Collections.Generic;
using ReserveTable.Models.Reviews;

namespace ReserveTable.App.Models.Restaurants
{
    public class RestaurantDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public double AverageRate { get; set; }

        public ICollection<AllReviewsForRestaurantViewModel> Reviews { get; set; }
    }
}
