namespace ReserveTable.Tests.Service
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using Services;
    using Common;
    using Xunit;
    using Domain;
    using Data;
    using Services.Models;

    public class RestaurantServiceTests
    {
        private IRestaurantService restaurantService;

        public RestaurantServiceTests()
        {
            AutoMapperFactory.InitializeMapper();
        }

        private List<City> GetCityData()
        {
            return new List<City>()
            {
                new City
                {
                    Id = "1",
                    Name = "Plovdiv",
                    Photo = "/src/plovdiv.jpg"
                }
            };
        }

        private List<Restaurant> GetRestaurantData()
        {
            return new List<Restaurant>()
            {
                new Restaurant
                {
                    Id="1",
                    Name = "Ego",
                    Address = "Egos Address",
                    CityId = "1",
                    PhoneNumber = "0888888888",
                    Photo = "/src/ego-photo.jpg"
                }
            };
        }

        private List<Review> GetReviewData()
        {
            return new List<Review>()
            {
                new Review
                {
                    Id = "1",
                    Comment ="Good",
                    Date = DateTime.Now,
                    Rate = 8,
                    RestaurantId = "1",
                    UserId = "1"
                },
                new Review
                {
                    Id = "2",
                    Comment ="Awesome",
                    Date = DateTime.Now,
                    Rate = 10,
                    RestaurantId = "1",
                    UserId = "2"
                }
            };
        }

        private List<ReserveTableUser> GetUserData()
        {
            return new List<ReserveTableUser>()
            {
                new ReserveTableUser
                {
                    Id = "1",
                    UserName = "pesho",
                    Email = "pesho@abv.bg"
                },
                new ReserveTableUser
                {
                    Id = "2",
                    UserName = "sasho",
                    Email = "sasho@abv.bg"
                }
            };
        }

        private async Task SeedData(ReserveTableDbContext dbContext)
        {
            dbContext.AddRange(GetCityData());
            dbContext.AddRange(GetRestaurantData());
            dbContext.AddRange(GetUserData());
            dbContext.AddRange(GetReviewData());
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateNewRestaurant_WithCorrectData_ShouldCreateSucessfully()
        {
            string errorMessage = "RestaurantService CreateNewRestaurant() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Name = "Happy",
                Address = "Center",
                CityId = "1",
                PhoneNumber = "08888888888",
                Photo = "/src/happy.jpg"
            };

            bool actualResult = await this.restaurantService.CreateNewRestaurant(restaurant);
            Assert.True(actualResult, errorMessage);
        }

        [Fact]
        public async Task CreateNewRestaurant_InNotExistentCity_ShouldThrowArgumentNullException()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Name = "Happy",
                Address = "Center",
                CityId = "2",
                PhoneNumber = "08888888888",
                Photo = "/src/happy.jpg"
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.restaurantService.CreateNewRestaurant(restaurant));
        }

        [Theory]
        [InlineData("AB")]
        [InlineData("Happy Happy Happy Happy Happy Happy")]
        public async Task CreateNewRestaurant_WithInvalidName_ShouldThrowArgumentException(string name)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Name = name,
                Address = "Center",
                CityId = "1",
                PhoneNumber = "08888888888",
                Photo = "/src/happy.jpg"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => this.restaurantService.CreateNewRestaurant(restaurant));
        }

        [Theory]
        [InlineData("Not")]
        [InlineData("This is very very long address.")]
        public async Task CreateNewRestaurant_WithInvalidAddress_ShouldThrowArgumentException(string address)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Name = "Happy",
                Address = address,
                CityId = "1",
                PhoneNumber = "08888888888",
                Photo = "/src/happy.jpg"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => this.restaurantService.CreateNewRestaurant(restaurant));
        }

        [Fact]
        public async Task CheckIfExists_WithExistingRestaurant_ShouldReturnTrue()
        {
            string errorMessage = "RestaurantService CheckIfExists() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Ego",
                Address = "Egos Address",
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/ego-photo.jpg"
            };

            string cityName = "Plovdiv";

            var actualResult = await restaurantService.CheckIfExists(restaurant, cityName);
            Assert.True(actualResult, errorMessage);
        }

        [Fact]
        public async Task CheckIfExists_WithNotExistingRestaurant_ShouldReturnFalse()
        {
            string errorMessage = "RestaurantService CheckIfExists() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Mc Donalds",
                Address = "Egos Address",
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/mcd-photo.jpg"
            };

            string cityName = "Plovdiv";

            var actualResult = await restaurantService.CheckIfExists(restaurant, cityName);
            Assert.False(actualResult, errorMessage);
        }

        [Fact]
        public async Task GetRestaurantByNameAndCity_WithData_ShouldReturnCorrectResult()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            string city = "Plovdiv";
            string restaurant = "Ego";
            RestaurantServiceModel expectedResult = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Ego",
                Address = "Egos Address",
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/ego-photo.jpg"
            };

            RestaurantServiceModel actualResult = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);

            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.Address, actualResult.Address);
            Assert.Equal(expectedResult.PhoneNumber, actualResult.PhoneNumber);
            Assert.Equal(expectedResult.Photo, actualResult.Photo);
            Assert.Equal(expectedResult.CityId, actualResult.CityId);
        }

        [Fact]
        public async Task GetRestaurantByNameAndCity_WithInvalidData_ShouldReturnNull()
        {
            string errorMessage = "RestaurantService GetRestaurantByNameAndCity() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            string city = "Plovdiv";
            string restaurant = "Happy";
            
            RestaurantServiceModel actualResult = await restaurantService.GetRestaurantByNameAndCity(city, restaurant);
            Assert.True(actualResult == null, errorMessage);
        }

        [Fact]
        public async Task GetAverageRate_WithData_ShouldReturnCorrectResult()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            var restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Ego",
                Address = "Egos Address",
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/ego-photo.jpg"
            };

            var reviews = context.Reviews.Where(r => r.Restaurant.Name == "Ego").ToList();
            var expectedResult = (reviews.Sum(r => r.Rate)) / (reviews.Count);
            var actualResult = await restaurantService.GetAverageRate(restaurant);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task SetNewRating_WithData_ShouldReturnCorrectResult()
        {
            string errorMessage = "RestaurantService SetNewRating() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            var restaurantId = "1";
            double rating = 10;

            var actualResult = await restaurantService.SetNewRating(restaurantId, rating);
            Assert.True(actualResult, errorMessage);
        }

        [Fact]
        public async Task SetNewRating_InNotExistantRestaurant_ShouldThrowArgumentNullException()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            var restaurantId = "10";
            double rating = 10;

            await Assert.ThrowsAsync<ArgumentNullException>(() => restaurantService.SetNewRating(restaurantId, rating));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(-1)]
        public async Task SetNewRating_WithInvalidRating_ShouldThrowArgumentException(double rating)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            var restaurantId = "1";

            await Assert.ThrowsAsync<ArgumentException>(() => restaurantService.SetNewRating(restaurantId, rating));
        }

        [Fact]
        public async Task GetRestaurantById_WithData_ShouldReturnCorrectResult()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            var restaurantId = "1";
            var expectedResult = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Ego",
                Address = "Egos Address",
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/ego-photo.jpg"
            };

            var actualResult = await restaurantService.GetRestaurantById(restaurantId);
            Assert.Equal(expectedResult.Name, actualResult.Name);
            Assert.Equal(expectedResult.Address, actualResult.Address);
            Assert.Equal(expectedResult.CityId, actualResult.CityId);
            Assert.Equal(expectedResult.PhoneNumber, actualResult.PhoneNumber);
            Assert.Equal(expectedResult.Photo, actualResult.Photo);
        }

        [Fact]
        public async Task GetRestaurantById_WithNotExistentRestaurant_ShouldReturnNull()
        {
            string errorMessage = "RestaurantService GetRestaurantById() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.restaurantService = new RestaurantService(context);
            await SeedData(context);

            var restaurantId = "10";
            var actualResult = await this.restaurantService.GetRestaurantById(restaurantId);
            Assert.True(actualResult == null, errorMessage);
        }
    }
}
