namespace ReserveTable.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Mapping;
    using ReserveTable.Models.Tables;
    using Models;

    public class TableService : ITableService
    {
        private readonly ReserveTableDbContext dbContext;

        public TableService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddTable(AddTableBindingModel model, RestaurantServiceModel restaurantServiceModel)
        {
            var table = new Table
            {
                SeatsCount = model.SeatsCount,
                RestaurantId = restaurantServiceModel.Id
            };

            await dbContext.Tables.AddAsync(table);
            var result = await dbContext.SaveChangesAsync();

            return result > 0; 
        }

        public async Task<IQueryable<TableServiceModel>> GetRestaurantTables(RestaurantServiceModel restaurant)
        {
            var tables = dbContext.Tables
                .Where(t => t.RestaurantId == restaurant.Id)
                .To<TableServiceModel>();

            return tables;
        }
    }
}
