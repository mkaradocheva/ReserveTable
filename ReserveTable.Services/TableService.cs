namespace ReserveTable.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using ReserveTable.Models.Tables;
    using Data;
    using Domain;
    using Mapping;
    using Models;
    using System;

    public class TableService : ITableService
    {
        private const int MinSeatsCount = 1;
        private const int MaxSeatsCount = 15;

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

            ValidateTable(table);
            CheckIfRestaurantExists(restaurantServiceModel);

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

        private void CheckIfRestaurantExists(RestaurantServiceModel restaurantServiceModel)
        {
            if (!dbContext.Restaurants.Any(restaurant => restaurant.Id == restaurantServiceModel.Id))
            {
                throw new ArgumentNullException(nameof(restaurantServiceModel));
            }
        }

        private static void ValidateTable(Table table)
        {
            if (table.SeatsCount < MinSeatsCount || table.SeatsCount > MaxSeatsCount)
            {
                throw new ArgumentException(nameof(table.SeatsCount));
            }
        }
    }
}
