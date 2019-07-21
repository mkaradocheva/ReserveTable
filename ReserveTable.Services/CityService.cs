using System.Linq;
using ReserveTable.Data;

namespace ReserveTable.Services
{
    public class CityService : ICityService
    {
        private readonly ReserveTableDbContext dbContext;

        public CityService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string FindCityByName(string cityName)
        {
            string cityId = dbContext.Cities
                .Where(c => c.Name == cityName)
                .Select(c => c.Id)
                .SingleOrDefault();

            return cityId;
        }
    }
}
