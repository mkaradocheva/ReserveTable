namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using Domain;
    using Models.Tables;

    public interface ITablesService
    {
        List<Table> GetRestaurantTables(Restaurant restaurant);

        void AddTable(AddTableBindingModel model, Restaurant restaurant);

    }
}
