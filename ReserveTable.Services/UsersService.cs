namespace ReserveTable.Services
{
    using System.Linq;
    using Data;
    using Domain;

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

        public ReserveTableUser GetUserByUsername(string username)
        {
            var user = dbContext.Users
                .Where(u => u.UserName == username)
                .FirstOrDefault();

            return user;
        }
    }
}
