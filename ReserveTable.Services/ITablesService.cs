using System.Collections.Generic;
using System.Threading.Tasks;
using ReserveTable.Domain;
using ReserveTable.Models.Tables;

namespace ReserveTable.Services
{
    public interface ITablesService
    {
        List<Table> GetRestaurantTables(Restaurant restaurant);

        void AddTable(AddTableBindingModel model, Restaurant restaurant);

    }
}
