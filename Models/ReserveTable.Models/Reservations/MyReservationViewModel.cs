namespace ReserveTable.Models.Reservations
{
    using System.Globalization;
    using AutoMapper;
    using Mapping;
    using ReserveTable.Services.Models;

    public class MyReservationViewModel : IMapFrom<ReservationServiceModel>, IHaveCustomMappings
    {
        private const string DateStringFormat = "dd/MM/yyyy HH:mm";

        public string Id { get; set; }

        public string Date { get; set; }

        public string Restaurant { get; set; }

        public string City { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ReservationServiceModel, MyReservationViewModel>()
                .ForMember(dest => dest.Date,
                opt => opt.MapFrom(origin => origin.ForDate.ToString(DateStringFormat, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.City,
                opt => opt.MapFrom(origin => origin.Restaurant.City.Name))
                .ForMember(dest => dest.Restaurant,
                opt => opt.MapFrom(origin => origin.Restaurant.Name));
        }
    }
}
