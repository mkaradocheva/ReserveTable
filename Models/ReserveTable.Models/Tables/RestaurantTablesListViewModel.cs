using System.Collections.Generic;

namespace ReserveTable.Models.Tables
{
    public class RestaurantTablesListViewModel
    {
        public string RestaurantName { get; set; }

        public string CityName { get; set; }

        public List<RestaurantTablesViewModel> Tables { get; set; }
    }
}
