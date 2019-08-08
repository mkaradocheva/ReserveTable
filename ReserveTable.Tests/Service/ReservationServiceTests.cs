namespace ReserveTable.Tests.Service
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Mapping;
    using Services;
    using Common;
    using Models.Reservations;
    using Domain;
    using Data;
    using Services.Models;

    public class ReservationServiceTests
    {
        private IReservationService reservationService;

        public ReservationServiceTests()
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
                    Photo = "/src/plovdiv.jpg",
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
                    Email = "pesho@abv.bg",
                    UserName = "pesho"
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
                    Address = "New Street",
                    AverageRating = 10,
                    CityId = "1",
                    Name = "Happy",
                    PhoneNumber = "0888888888",
                    Photo = "/src/happy.jpg",
                    Tables = new List<Table>()
                    {
                        new Table
                        {
                            Id = "1",
                    SeatsCount = 2,
                    RestaurantId = "1"
                        }
                    }
                }
            };
        }

        private List<Reservation> GetReservationData()
        {
            return new List<Reservation>()
            {
                new Reservation
                {
                    Id = "1",
                    ForDate = new DateTime(2019, 9, 5, 20, 0, 0),
                    RestaurantId = "1",
                    IsCancelled = false,
                    SeatsCount = 2,
                    TableId = "1",
                    UserId = "1",
                    Restaurant = new Restaurant
                    {
Id = "1",
                    Address = "New Street",
                    AverageRating = 10,
                    CityId = "1",
                    Name = "Happy",
                    PhoneNumber = "0888888888",
                    Photo = "/src/happy.jpg",
                    Tables = new List<Table>()
                    {
                        new Table
                        {
                            Id = "1",
                    SeatsCount = 2,
                    RestaurantId = "1"
                        }
                    }
                    }
                }
            };
        }

        private async Task SeedData(ReserveTableDbContext dbContext)
        {
            dbContext.AddRange(GetCityData());
            dbContext.AddRange(GetRestaurantData());
            dbContext.AddRange(GetUserData());
            dbContext.AddRange(GetReservationData());
            await dbContext.SaveChangesAsync();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public async Task MakeReservation_WithCorrectData_ShouldCreateSucessfully(int seats)
        {
            string errorMessage = "ReservationService MakeReservation() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);

            CreateReservationBindingModel reservationBindingModel = new CreateReservationBindingModel
            {
                Date = "09/08/2019 20:00:00",
                SeatsCount = seats,
                Restaurant = "Happy"
            };

            ReserveTableUserServiceModel user = new ReserveTableUserServiceModel
            {
                Id = "1",
                Email = "pesho@abv.bg",
                UserName = "pesho"
            };

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Address = "New Street",
                AverageRating = 10,
                CityId = "1",
                Name = "Happy",
                PhoneNumber = "0888888888",
                Photo = "/src/happy.jpg",
                Tables = new List<TableServiceModel>()
                    {
                        new TableServiceModel
                        {
                            Id = "1",
                    SeatsCount = 2,
                    RestaurantId = "1",
                    Reservations = new List<ReservationServiceModel>()
                        }
                    }
            };

            var expectedResult = new ReservationServiceModel
            {
                ForDate = new DateTime(2019, 08, 09, 20, 0, 0),
                Restaurant = new RestaurantServiceModel
                {
                    Id = "1",
                    Address = "New Street",
                    AverageRating = 10,
                    CityId = "1",
                    Name = "Happy",
                    PhoneNumber = "0888888888",
                    Photo = "/src/happy.jpg",
                    Tables = new List<TableServiceModel>()
                    {
                        new TableServiceModel
                        {
                            Id = "1",
                    SeatsCount = 2,
                    RestaurantId = "1",
                    Reservations = new List<ReservationServiceModel>()
                        }
                    }
                },
                SeatsCount = seats,
                UserId = "1"
            };
            var actualResult = await reservationService.MakeReservation(reservationBindingModel, user, restaurant);

            Assert.True(expectedResult.SeatsCount == actualResult.SeatsCount, errorMessage + " " + "Seats Count is not returned properly");
            Assert.True(expectedResult.ForDate == actualResult.ForDate, errorMessage + " " + "For Date is not returned properly");
            Assert.True(expectedResult.UserId == actualResult.UserId, errorMessage + " " + "User Id is not returned properly");
        }

        [Fact]
        public async Task MakeReservation_WithNoAvailableTables_ShouldReturnNull()
        {
            string errorMessage = "ReservationService MakeReservation() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);

            CreateReservationBindingModel reservationBindingModel = new CreateReservationBindingModel
            {
                Date = "09/08/2019 20:00:00",
                SeatsCount = 4,
                Restaurant = "Happy"
            };

            ReserveTableUserServiceModel user = new ReserveTableUserServiceModel
            {
                Id = "1",
                Email = "pesho@abv.bg",
                UserName = "pesho"
            };

            RestaurantServiceModel restaurant = new RestaurantServiceModel
            {
                Id = "1",
                Address = "New Street",
                AverageRating = 10,
                CityId = "1",
                Name = "Happy",
                PhoneNumber = "0888888888",
                Photo = "/src/happy.jpg",
                Tables = new List<TableServiceModel>()
                    {
                        new TableServiceModel
                        {
                            Id = "1",
                    SeatsCount = 2,
                    RestaurantId = "1",
                    Reservations = new List<ReservationServiceModel>()
                        }
                    }
            };

            var actualResult = await reservationService.MakeReservation(reservationBindingModel, user, restaurant);
            Assert.True(actualResult == null, errorMessage);
        }

        [Fact]
        public async Task GetMyReservations_WithData_ShouldReturnCorrectResults()
        {
            string errorMessage = "ReservationService GetMyReservations() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);

            List<ReservationServiceModel> expectedResults = GetReservationData().To<ReservationServiceModel>().ToList();
            var reservations = await reservationService.GetMyReservations("pesho");
            List<ReservationServiceModel> actualResults = reservations.ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var actualResult = actualResults[i];
                var expectedResult = expectedResults[i];

                Assert.True(actualResult.RestaurantId == expectedResult.RestaurantId, errorMessage + " " + "Restaurant Id is not returned properly.");
                Assert.True(actualResult.SeatsCount == expectedResult.SeatsCount, errorMessage + " " + "Seats Count is not returned properly.");
                Assert.True(actualResult.IsCancelled == expectedResult.IsCancelled, errorMessage + " " + "Is Cancelled is not returned properly.");
                Assert.True(actualResult.TableId == expectedResult.TableId, errorMessage + " " + "Table Id is not returned properly.");
                Assert.True(actualResult.ForDate == expectedResult.ForDate, errorMessage + " " + "For Date is not returned properly.");
            }
        }

        [Fact]
        public async Task CancelReservation_WithData_ShouldReturnTrue()
        {
            string errorMessage = "ReservationService CancelReservation() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);
            string reservationId = "1";
            var actualResult = await reservationService.CancelReservation(reservationId);

            Assert.True(actualResult, errorMessage);
        }

        [Fact]
        public async Task CancelReservation_WithInvalidReservationId_ShouldThrowArgumentNullException()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);
            string reservationId = "2";

            await Assert.ThrowsAsync<ArgumentNullException>(() => reservationService.CancelReservation(reservationId));
        }

        [Fact]
        public async Task GetReservationById_WithData_ShouldReturnCorrectResults()
        {
            string errorMessage = "ReservationService GetReservationById() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);

            string reservationId = "1";
            var actualResult = await reservationService.GetReservationById(reservationId);
            ReservationServiceModel expectedResult = new ReservationServiceModel
            {
                Id = "1",
                ForDate = new DateTime(2019, 9, 5, 20, 0, 0),
                RestaurantId = "1",
                IsCancelled = false,
                SeatsCount = 2,
                TableId = "1",
                UserId = "1",
                Restaurant = new RestaurantServiceModel
                {
                    Id = "1",
                    Address = "New Street",
                    AverageRating = 10,
                    CityId = "1",
                    Name = "Happy",
                    PhoneNumber = "0888888888",
                    Photo = "/src/happy.jpg",
                    Tables = new List<TableServiceModel>()
                    {
                        new TableServiceModel
                        {
                            Id = "1",
                    SeatsCount = 2,
                    RestaurantId = "1"
                        }
                    }
                }
            };

            Assert.True(actualResult.RestaurantId == expectedResult.RestaurantId, errorMessage + " " + "Restaurant Id is not returned properly.");
            Assert.True(actualResult.SeatsCount == expectedResult.SeatsCount, errorMessage + " " + "Seats Count is not returned properly.");
            Assert.True(actualResult.IsCancelled == expectedResult.IsCancelled, errorMessage + " " + "Is Cancelled is not returned properly.");
            Assert.True(actualResult.TableId == expectedResult.TableId, errorMessage + " " + "Table Id is not returned properly.");
            Assert.True(actualResult.ForDate == expectedResult.ForDate, errorMessage + " " + "For Date is not returned properly.");
        }

        [Fact]
        public async Task GetReservationById_WithInvalidId_ShouldThrowArgumentNullException()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);
            string reservationId = "2";
            await Assert.ThrowsAsync<ArgumentNullException>(() => reservationService.GetReservationById(reservationId));
        }

        [Fact]
        public async Task IsDateValid_WithValidData_ShouldReturnTrue()
        {
            string errorMessage = "ReservationService IsDateValid() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);

            DateTime validDateTime = DateTime.Now.AddDays(1);

            var actualResult = await reservationService.IsDateValid(validDateTime);
            Assert.True(actualResult, errorMessage);
        }

        [Fact]
        public async Task IsDateValid_WithInvalidData_ShouldReturnFalse()
        {
            string errorMessage = "ReservationService IsDateValid() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reservationService = new ReservationService(context);
            await SeedData(context);

            DateTime invalidDateTime = DateTime.Now.AddDays(-1);

            var actualResult = await reservationService.IsDateValid(invalidDateTime);
            Assert.False(actualResult, errorMessage);
        }
    }
}
