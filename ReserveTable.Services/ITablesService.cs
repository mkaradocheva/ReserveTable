using System;
using System.Collections.Generic;
using ReserveTable.Domain;

namespace ReserveTable.Services
{
    public interface ITablesService
    {
        List<Table> GetRestaurantTables(Restaurant restaurant);
    }
}
