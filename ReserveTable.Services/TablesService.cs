namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Models.Tables;

    public class TablesService : ITablesService
    {
        private readonly ReserveTableDbContext dbContext;

        public TablesService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddTable(AddTableBindingModel model, Restaurant restaurant)
        {
            var table = new Table
            {
                SeatsCount = model.SeatsCount,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id
            };

            await dbContext.Tables.AddAsync(table);
            var result = await dbContext.SaveChangesAsync();

            return result > 0; 
        }

        public async Task<List<Table>> GetRestaurantTables(Restaurant restaurant)
        {
            var tables = await dbContext.Tables
                .Where(t => t.RestaurantId == restaurant.Id)
                .ToListAsync();

            return tables;
        }
    }
}
