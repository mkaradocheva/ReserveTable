using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReserveTable.Data
{
    public class ReserveTableDbContext : IdentityDbContext
    {
        public ReserveTableDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
