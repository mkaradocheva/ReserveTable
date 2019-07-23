using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReserveTable.Data;
using ReserveTable.Domain;

namespace ReserveTable.Services
{
    public class TablesService : ITablesService
    {
        private readonly ReserveTableDbContext dbContext;

        public TablesService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
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
