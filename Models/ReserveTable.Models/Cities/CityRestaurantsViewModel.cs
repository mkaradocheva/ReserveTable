namespace ReserveTable.App.Models.Cities
{
    using System.Collections.Generic;
    using Restaurants;

    public class CityRestaurantsViewModel
    {
        public List<RestaurantsViewModel> Restaurants { get; set; }

        public string CityName { get; set; }
    }
}
