using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReserveTable.Data;
using ReserveTable.Domain;
using ReserveTable.Models.Tables;

namespace ReserveTable.Services
{
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
