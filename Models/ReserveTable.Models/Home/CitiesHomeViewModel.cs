using ReserveTable.Mapping;
using ReserveTable.Services.Models;

namespace ReserveTable.Models.Home
{
    public class CitiesHomeViewModel : IMapFrom<CityServiceModel>
    {
        public string Name { get; set; }

        public string Photo { get; set; }
    }
}
