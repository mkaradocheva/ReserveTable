using ReserveTable.Data;
using ReserveTable.Domain;

namespace ReserveTable.Services
{
    public class UsersService : IUsersService
    {
        private readonly ReserveTableDbContext dbContext;

        public UsersService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ReserveTableUser GetUserById(string id)
        {
            var user = dbContext.Users.Find(id);

            return user;
        }
    }
}
