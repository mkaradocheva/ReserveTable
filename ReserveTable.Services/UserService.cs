namespace ReserveTable.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly ReserveTableDbContext dbContext;

        public UserService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ReserveTableUser> GetUserById(string id)
        {
            var user = await dbContext.Users.FindAsync(id);

            return user;
        }

        public async Task<ReserveTableUser> GetUserByUsername(string username)
        {
            var user = await dbContext.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
