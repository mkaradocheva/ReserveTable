namespace ReserveTable.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Models;
    using Mapping;

    public class UserService : IUserService
    {
        private readonly ReserveTableDbContext dbContext;

        public UserService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ReserveTableUserServiceModel> GetUserById(string id)
        {
            var user = await dbContext.Users.FindAsync(id);
            ReserveTableUserServiceModel userServiceModel = AutoMapper.Mapper.Map<ReserveTableUserServiceModel>(user);

            return userServiceModel;
        }

        public async Task<ReserveTableUserServiceModel> GetUserByUsername(string username)
        {
            var user = await dbContext.Users
                .Where(u => u.UserName == username)
                .To<ReserveTableUserServiceModel>()
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
