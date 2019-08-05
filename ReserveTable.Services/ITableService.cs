namespace ReserveTable.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using ReserveTable.Models.Tables;
    using ReserveTable.Services.Models;

    public interface ITableService
    {
        Task<IQueryable<TableServiceModel>> GetRestaurantTables(RestaurantServiceModel restaurant);

        Task<bool> AddTable(AddTableBindingModel model, RestaurantServiceModel restaurantServiceModel);
    }
}
