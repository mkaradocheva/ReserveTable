using System.Collections.Generic;
using ReserveTable.App.Models.Restaurants;

namespace ReserveTable.App.Models.Cities
{
    public class CityRestaurantsViewModel
    {
        public List<RestaurantsViewModel> RestaurantsNames { get; set; }

        public string CityName { get; set; }
    }
}
