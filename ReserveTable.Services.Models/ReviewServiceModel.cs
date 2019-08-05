namespace ReserveTable.Services.Models
{
    using System;
    using ReserveTable.Domain;
    using ReserveTable.Mapping;

    public class ReviewServiceModel : IMapTo<Review>, IMapFrom<Review>
    {
        public string Id { get; set; }

        public string Comment { get; set; }

        public double Rate { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public ReserveTableUserServiceModel User { get; set; }

        public string RestaurantId { get; set; }
        public RestaurantServiceModel Restaurant { get; set; }
    }
}
