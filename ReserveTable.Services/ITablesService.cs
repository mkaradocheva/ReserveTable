namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;
    using Models.Tables;

    public interface ITablesService
    {
        Task<List<Table>> GetRestaurantTables(Restaurant restaurant);

        Task<bool> AddTable(AddTableBindingModel model, Restaurant restaurant);

    }
}
