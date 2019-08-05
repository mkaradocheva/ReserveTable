namespace ReserveTable.Tests.Service
{
    using System.Linq;
    using System.Threading.Tasks;
    using ReserveTable.Services;
    using ReserveTable.Services.Models;
    using ReserveTable.Tests.Common;
    using Xunit;
    using Mapping;
    using ReserveTable.Domain;
    using System.Collections.Generic;
    using ReserveTable.Data;

    public class UserServiceTests
    {
        private IUserService userService;

        public UserServiceTests()
        {
            AutoMapperFactory.InitializeMapper();
        }

        private List<ReserveTableUser> GetData()
        {
            return new List<ReserveTableUser>()
            {
                new ReserveTableUser
                {
                    UserName = "pesho",
                    Email = "pesho@abv.bg",
                },
                new ReserveTableUser
                {
                    UserName = "tosho",
                    Email = "tosho@abv.bg"
                }
            };
        }

        private async Task SeedData(ReserveTableDbContext context)
        {
            context.AddRange(GetData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetUserById_WithExistentId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserService GetById() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.userService = new UserService(context);

            ReserveTableUserServiceModel expectedData = context.Users.First().To<ReserveTableUserServiceModel>();
            ReserveTableUserServiceModel actualData = await this.userService.GetUserById(expectedData.Id);

            Assert.True(expectedData.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedData.UserName == actualData.UserName, errorMessagePrefix + " " + "UserName is not returned properly.");
            Assert.True(expectedData.Email == actualData.Email, errorMessagePrefix + " " + "Price is not returned properly.");
        }

        [Fact]
        public async Task GetUserById_WithNonExistentId_ShouldReturnNull()
        {
            string errorMessage = "UserService GetById() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.userService = new UserService(context);

            ReserveTableUserServiceModel actualData = await this.userService.GetUserById("invalid");

            Assert.True(actualData == null, errorMessage);
        }

        [Fact]
        public async Task GetUserByUsername_WithExistentUsername_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserService GetUserByUsername() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.userService = new UserService(context);

            ReserveTableUserServiceModel expectedData = context.Users.First().To<ReserveTableUserServiceModel>();
            ReserveTableUserServiceModel actualData = await this.userService.GetUserByUsername(expectedData.UserName);

            Assert.True(expectedData.Id == actualData.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedData.UserName == actualData.UserName, errorMessagePrefix + " " + "UserName is not returned properly.");
            Assert.True(expectedData.Email == actualData.Email, errorMessagePrefix + " " + "Price is not returned properly.");
        }

        [Fact]
        public async Task GetUserByUsername_WithNonExistentUsername_ShouldReturnNull()
        {
            string errorMessage = "UserService GetUserByUsername() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.userService = new UserService(context);

            ReserveTableUserServiceModel actualData = await this.userService.GetUserByUsername("invalid");

            Assert.True(actualData == null, errorMessage);
        }
    }
}
