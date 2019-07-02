using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ReserveTable.Domain
{
    public class ReserveTableUser : IdentityUser
    {
        public ReserveTableUser()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public ReserveTableUserRole UserRole { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
