namespace ReserveTable.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Domain;
    using Models.Tables;

    public class TablesService : ITablesService
    {
        private readonly ReserveTableDbContext dbContext;

        public TablesService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddTable(AddTableBindingModel model, Restaurant restaurant)
        {
            var table = new Table
            {
                SeatsCount = model.SeatsCount,
                Restaurant = restaurant,
                RestaurantId = restaurant.Id
            };

            dbContext.Tables.Add(table);
            dbContext.SaveChanges();
        }

        public List<Table> GetRestaurantTables(Restaurant restaurant)
        {
            var tables = dbContext.Tables
                .Where(t => t.RestaurantId == restaurant.Id)
                .ToList();

            return tables;
        }
    }
}
