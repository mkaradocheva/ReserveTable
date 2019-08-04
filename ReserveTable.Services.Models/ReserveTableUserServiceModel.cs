namespace ReserveTable.Services.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ReserveTableUserServiceModel : IdentityUser
    {
        public ReserveTableUserRoleServiceModel UserRole { get; set; }

        public ICollection<ReservationServiceModel> Reservations { get; set; }

        public ICollection<ReviewServiceModel> Reviews { get; set; }
    }
}
