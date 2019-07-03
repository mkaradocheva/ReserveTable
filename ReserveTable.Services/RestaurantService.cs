using System.Collections.Generic;
using System.Linq;
using ReserveTable.Data;

namespace ReserveTable.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ReserveTableDbContext dbContext;

        public RestaurantService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICollection<string> GetAllCitiesNames()
        {
            var allCitiesNames = dbContext.Cities
                .Select(c => c.Name)
                .ToList();

            return allCitiesNames;
        }
    }
}
