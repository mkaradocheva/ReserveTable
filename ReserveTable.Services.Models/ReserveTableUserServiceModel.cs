namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ReserveTable.Domain;
    using ReserveTable.Mapping;

    public class ReserveTableUserServiceModel : IdentityUser, IMapTo<ReserveTableUser>, IMapFrom<ReserveTableUser>
    {
        public ReserveTableUserRoleServiceModel UserRole { get; set; }

        public ICollection<ReservationServiceModel> Reservations { get; set; }

        public ICollection<ReviewServiceModel> Reviews { get; set; }
    }
}
