namespace ReserveTable.Tests.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using Data;
    using Domain;
    using Models.Tables;
    using Services;
    using Services.Models;
    using Common;
    using Mapping;
    using System;

    public class TableServiceTests
    {
        private ITableService tableService;

        public TableServiceTests()
        {
            AutoMapperFactory.InitializeMapper();
        }

        private List<Table> GetTableData()
        {
            return new List<Table>()
                {
                new Table
                {
                    Id = "1",
                    RestaurantId = "1",
                    SeatsCount = 5
                },
                new Table
                {
                    Id = "2",
                    RestaurantId = "1",
                    SeatsCount = 2
                }
            };
        }

        private List<Restaurant> GetRestaurantData()
        {
            return new List<Restaurant>()
            {
                new Restaurant
                {
                    Id = "1",
                    Address = "Ivan Vazov",
                    AverageRating = 10,
                    Name = "Happy",
                    PhoneNumber = "08888888888",
                    Photo = "src/happy-photo.jpg",
                    CityId = "1"
                },
                new Restaurant
                {
                    Id = "2",
                    Address = "Hristo Botev",
                    AverageRating = 10,
                    Name = "KFC",
                    PhoneNumber = "08888888889",
                    Photo = "src/KFC-photo.jpg",
                    CityId = "1"
                },
            };
        }

        private List<City> GetCityData()
        {
            return new List<City>()
            {
                new City
                {
                    Id = "1",
                    Name = "Plovdiv",
                    Photo = "src/plovdiv.jpg",
                }
            };
        }

        private async Task SeedData(ReserveTableDbContext context)
        {
            context.AddRange(GetRestaurantData());
            context.AddRange(GetCityData());
            context.AddRange(GetTableData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddTable_WithCorrectData_ShouldCreateSucessfully()
        {
            var errorMessage = "Table service AddTable() method is not working correctly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.tableService = new TableService(context);
            await SeedData(context);

            AddTableBindingModel model = new AddTableBindingModel
            {
                SeatsCount = 5
            };

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Address = "Ivan Vazov",
                AverageRating = 10,
                Name = "Happy",
                PhoneNumber = "08888888888",
                Photo = "src/happy-photo.jpg",
                CityId = "1"
            };

            var actualResult = await this.tableService.AddTable(model, restaurant);

            Assert.True(actualResult, errorMessage);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(200)]
        [InlineData(-1)]
        [InlineData(16)]
        public async Task AddTable_WithInvalidSeats_ShouldThrowArgumentException(int seatsCount)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.tableService = new TableService(context);
            await SeedData(context);

            AddTableBindingModel model = new AddTableBindingModel
            {
                SeatsCount = seatsCount
            };

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Address = "Ivan Vazov",
                AverageRating = 10,
                Name = "Happy",
                PhoneNumber = "08888888888",
                Photo = "src/happy-photo.jpg",
                CityId = "1"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => this.tableService.AddTable(model, restaurant));
        }

        [Fact]
        public async Task AddTable_InInvalidRestaurant_ShouldNotCreate()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.tableService = new TableService(context);
            await SeedData(context);

            AddTableBindingModel model = new AddTableBindingModel
            {
                SeatsCount = 5
            };

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "3",
                Address = "Ivan Vazov",
                AverageRating = 10,
                Name = "Happy",
                PhoneNumber = "08888888888",
                Photo = "src/happy-photo.jpg",
                CityId = "1"
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => tableService.AddTable(model, restaurant));
        }

        [Fact]
        public async Task GetRestaurantTables_WithData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "Table service GetAllProducts() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.tableService = new TableService(context);

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Address = "Ivan Vazov",
                AverageRating = 10,
                Name = "Happy",
                PhoneNumber = "08888888888",
                Photo = "src/happy-photo.jpg",
                CityId = "1"
            };

            var tables = await this.tableService.GetRestaurantTables(restaurant);
            List<TableServiceModel> actualResults = tables.ToList();
            List<TableServiceModel> expectedResults = GetTableData().To<TableServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.SeatsCount == actualEntry.SeatsCount, errorMessagePrefix + " " + "Seats count is not returned properly.");
                Assert.True(expectedEntry.RestaurantId == actualEntry.RestaurantId, errorMessagePrefix + " " + "Restaurant Id is not returned properly.");
            }
        }
    }
}
