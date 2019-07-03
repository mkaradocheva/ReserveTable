using System.Collections.Generic;

namespace ReserveTable.Services
{
    public interface IRestaurantService
    {
        ICollection<string> GetAllCitiesNames();
    }
}
