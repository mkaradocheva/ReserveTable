using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReserveTable.Domain;

namespace ReserveTable.Data
{
    public class ReserveTableDbContext : IdentityDbContext<ReserveTableUser, ReserveTableUserRole, string>
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public ReserveTableDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ReserveTableDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
