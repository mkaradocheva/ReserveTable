namespace ReserveTable.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Domain;

    public class ReserveTableDbContext : IdentityDbContext<ReserveTableUser, ReserveTableUserRole, string>
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public ReserveTableDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ReserveTableDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ReserveTableUser>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(u => u.UserId);

            builder.Entity<ReserveTableUser>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(u => u.UserId);

            builder.Entity<Table>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Table)
                .HasForeignKey(t => t.TableId);

            builder.Entity<City>()
                .HasMany(c => c.Restaurants)
                .WithOne(r => r.City)
                .HasForeignKey(c => c.CityId);

            builder.Entity<Restaurant>()
                .HasMany(r => r.Tables)
                .WithOne(t => t.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            base.OnModelCreating(builder);
        }
    }
}
