namespace ReserveTable.Models.Tables
{
    using System.Collections.Generic;

    public class RestaurantTablesListViewModel
    {
        public string Id { get; set; }

        public string RestaurantName { get; set; }

        public string CityName { get; set; }

        public List<RestaurantTablesViewModel> Tables { get; set; }
    }
}
