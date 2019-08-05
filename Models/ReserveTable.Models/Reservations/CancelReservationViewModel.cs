namespace ReserveTable.Models.Reservations
{
    using AutoMapper;
    using ReserveTable.Mapping;
    using ReserveTable.Services.Models;
    public class CancelReservationViewModel : IMapFrom<ReservationServiceModel>, IHaveCustomMappings
    {
        private const string DateStringFormat = "dd/MM/yyyy HH:mm";

        public string Date { get; set; }

        public string Restaurant { get; set; }

        public string City { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ReservationServiceModel, CancelReservationViewModel>()
                .ForMember(dest => dest.Date,
                opt => opt.MapFrom(origin => origin.ForDate.ToString(DateStringFormat)))
                .ForMember(dest => dest.City,
                opt => opt.MapFrom(origin => origin.Restaurant.City.Name))
                .ForMember(dest => dest.Restaurant,
                opt => opt.MapFrom(origin => origin.Restaurant.Name));
        }
    }
}
