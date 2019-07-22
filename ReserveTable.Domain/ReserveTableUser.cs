using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ReserveTable.Domain
{
    public class ReserveTableUser : IdentityUser
    {
        public ReserveTableUser()
        {
            this.Reservations = new HashSet<Reservation>();
            this.Reviews = new HashSet<Review>();
        }

        public ReserveTableUserRole UserRole { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
