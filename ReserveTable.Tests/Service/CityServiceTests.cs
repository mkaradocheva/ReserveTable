namespace ReserveTable.Tests.Service
{
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Services;
    using Common;
    using Services.Models;
    using System.Collections.Generic;
    using Domain;
    using Data;
    using System.Linq;
    using Mapping;

    public class CityServiceTests
    {
        private ICityService cityService;

        public CityServiceTests()
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
                },
                new City
                {
                    Id = "2",
                    Name = "Sofia",
                    Photo = "/src/Sofia.jpg"
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
                    Name = "Happy",
                    Address = "Ivan Vazov",
                    AverageRating = 10,
                    CityId = "1",
                    PhoneNumber = "0888888888",
                    Photo = "/src/Happy.jpg"
                },
                new Restaurant
                {
                    Id = "2",
                    Name = "Not Happy",
                    Address = "Hristo Botev",
                    AverageRating = 5,
                    CityId = "2",
                    PhoneNumber = "0888888889",
                    Photo = "/src/NotHappy.jpg"
                },
                new Restaurant
                {
                    Id = "3",
                    Name = "KFC",
                    Address = "Vasil Levski",
                    AverageRating = 7,
                    CityId = "1",
                    PhoneNumber = "0888888899",
                    Photo = "/src/KFC.jpg"
                }
            };
        }

        private async Task Seed(ReserveTableDbContext context)
        {
            context.AddRange(GetCityData());
            context.AddRange(GetRestaurantData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task AddCity_WithCorrectData_ShouldCreateSucessfully()
        {
            string errorMessage = "CityService AddCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);

            CityServiceModel city = new CityServiceModel
            {
                Name = "Varna",
                Photo = "/src/varna.jpg",
            };

            bool actualResult = await this.cityService.AddCity(city);
            Assert.True(actualResult, errorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ab")]
        [InlineData("Varna Varna Varna Varna")]
        public async Task AddCity_WithInvalidName_ShouldThrowArgumentException(string cityName)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);

            CityServiceModel city = new CityServiceModel
            {
                Name = cityName,
                Photo = "/src/varna.jpg",
            };

            await Assert.ThrowsAsync<ArgumentException>(() => this.cityService.AddCity(city));
        }

        [Fact]
        public async Task CheckIfExists_WithExistingCity_ShouldReturnTrue()
        {
            string errorMessage = "CityService CheckIfExists() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);
            await Seed(context);

            CityServiceModel city = new CityServiceModel
            {
                Name = "Plovdiv",
                Photo = "/src/plovdiv.jpg"
            };

            var actualResult = await cityService.CheckIfExists(city);
            Assert.True(actualResult, errorMessage);
        }

        [Fact]
        public async Task CheckIfExists_WithNotExistentCity_ShouldReturnFalse()
        {
            string errorMessage = "CityService CheckIfExists() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);
            await Seed(context);

            CityServiceModel city = new CityServiceModel
            {
                Name = "Varna",
                Photo = "/src/varna.jpg"
            };

            var actualResult = await cityService.CheckIfExists(city);
            Assert.False(actualResult, errorMessage);
        }

        [Fact]
        public async Task GetAllCities_WithData_ShouldReturnCorrectResults()
        {
            string errorMessage = "CityService GetAllCities() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);
            await Seed(context);

            var cities = await this.cityService.GetAllCities();
            List<CityServiceModel> actualResults = cities.ToList();
            List<CityServiceModel> expectedResults = GetCityData().To<CityServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessage + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Photo == actualEntry.Photo, errorMessage + " " + "Photo is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllCitiesNames_WithData_ShouldReturnCorrectResults()
        {
            string errorMessage = "CityService GetAllCitiesNames() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);
            await Seed(context);

            var cities = await this.cityService.GetAllCitiesNames();
            List<string> actualResults = cities.ToList();
            List<string> expectedResults = GetCityData().Select(city => city.Name).ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry == actualEntry, errorMessage);
            }
        }

        [Fact]
        public async Task GetCityByName_WithExistentName_ShouldReturnCorrectResults()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);
            await Seed(context);

            var expectedResult = "1";
            var actualResult = await cityService.GetCityByName("Plovdiv");

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetCityByName_WithNotExistentName_ShouldReturnNull()
        {
            var errorMessage = "CityService GetCityByName() method does not work properly.";
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);
            await Seed(context);

            var actualResult = await cityService.GetCityByName("Vidin");

            Assert.True(actualResult == null, errorMessage);
        }

        [Fact]
        public async Task GetRestaurantsInCity_WithData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "CityService GetRestaurantsInCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await Seed(context);
            this.cityService = new CityService(context);

            var restaurantsInCity = await this.cityService.GetRestaurantsInCity("Plovdiv");

            var restaurantInPlovdiv = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Happy",
                Address = "Ivan Vazov",
                AverageRating = 10,
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/Happy.jpg"
            };

            var restaurantInPlovdiv2 = new RestaurantServiceModel
            {
                Id = "3",
                Name = "KFC",
                Address = "Vasil Levski",
                AverageRating = 7,
                CityId = "1",
                PhoneNumber = "0888888899",
                Photo = "/src/KFC.jpg"
            };

            List<RestaurantServiceModel> actualResults = restaurantsInCity.ToList();
            List<RestaurantServiceModel> expectedResults = new List<RestaurantServiceModel>();
            expectedResults.Add(restaurantInPlovdiv);
            expectedResults.Add(restaurantInPlovdiv2);

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Address == actualEntry.Address, errorMessagePrefix + " " + "Address is not returned properly.");
                Assert.True(expectedEntry.PhoneNumber == actualEntry.PhoneNumber, errorMessagePrefix + " " + "Phone Number is not returned properly.");
                Assert.True(expectedEntry.CityId == actualEntry.CityId, errorMessagePrefix + " " + "City Id is not returned properly.");
                Assert.True(expectedEntry.AverageRating == actualEntry.AverageRating, errorMessagePrefix + " " + "Average Rating is not returned properly.");
                Assert.True(expectedEntry.Photo == actualEntry.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRestaurantsInCity_WithDataRatingAscending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "CityService GetRestaurantsInCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await Seed(context);
            this.cityService = new CityService(context);

            var restaurantInPlovdiv = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Happy",
                Address = "Ivan Vazov",
                AverageRating = 10,
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/Happy.jpg"
            };

            var restaurantInPlovdiv2 = new RestaurantServiceModel
            {
                Id = "3",
                Name = "KFC",
                Address = "Vasil Levski",
                AverageRating = 7,
                CityId = "1",
                PhoneNumber = "0888888899",
                Photo = "/src/KFC.jpg"
            };

            var restaurantsInCity = await this.cityService.GetRestaurantsInCity("Plovdiv", "rating-lowest-to-highest");

            List<RestaurantServiceModel> actualResults = restaurantsInCity.ToList();
            List<RestaurantServiceModel> expectedResults = new List<RestaurantServiceModel>
            {
                restaurantInPlovdiv,
                restaurantInPlovdiv2
            }
            .OrderBy(r => r.AverageRating)
            .ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Address == actualEntry.Address, errorMessagePrefix + " " + "Address is not returned properly.");
                Assert.True(expectedEntry.PhoneNumber == actualEntry.PhoneNumber, errorMessagePrefix + " " + "Phone Number is not returned properly.");
                Assert.True(expectedEntry.CityId == actualEntry.CityId, errorMessagePrefix + " " + "City Id is not returned properly.");
                Assert.True(expectedEntry.AverageRating == actualEntry.AverageRating, errorMessagePrefix + " " + "Average Rating is not returned properly.");
                Assert.True(expectedEntry.Photo == actualEntry.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRestaurantsInCity_WithDataRatingDescending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "CityService GetRestaurantsInCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await Seed(context);
            this.cityService = new CityService(context);

            var restaurantInPlovdiv = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Happy",
                Address = "Ivan Vazov",
                AverageRating = 10,
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/Happy.jpg"
            };

            var restaurantInPlovdiv2 = new RestaurantServiceModel
            {
                Id = "3",
                Name = "KFC",
                Address = "Vasil Levski",
                AverageRating = 7,
                CityId = "1",
                PhoneNumber = "0888888899",
                Photo = "/src/KFC.jpg"
            };

            var restaurantsInCity = await this.cityService.GetRestaurantsInCity("Plovdiv", "rating-highest-to-lowest");

            List<RestaurantServiceModel> actualResults = restaurantsInCity.ToList();
            List<RestaurantServiceModel> expectedResults = new List<RestaurantServiceModel>
            {
                restaurantInPlovdiv,
                restaurantInPlovdiv2
            }
            .OrderByDescending(r => r.AverageRating)
            .ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Address == actualEntry.Address, errorMessagePrefix + " " + "Address is not returned properly.");
                Assert.True(expectedEntry.PhoneNumber == actualEntry.PhoneNumber, errorMessagePrefix + " " + "Phone Number is not returned properly.");
                Assert.True(expectedEntry.CityId == actualEntry.CityId, errorMessagePrefix + " " + "City Id is not returned properly.");
                Assert.True(expectedEntry.AverageRating == actualEntry.AverageRating, errorMessagePrefix + " " + "Average Rating is not returned properly.");
                Assert.True(expectedEntry.Photo == actualEntry.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRestaurantsInCity_WithDataAlphabeticallyAscending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "CityService GetRestaurantsInCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await Seed(context);
            this.cityService = new CityService(context);

            var restaurantInPlovdiv = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Happy",
                Address = "Ivan Vazov",
                AverageRating = 10,
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/Happy.jpg"
            };

            var restaurantInPlovdiv2 = new RestaurantServiceModel
            {
                Id = "3",
                Name = "KFC",
                Address = "Vasil Levski",
                AverageRating = 7,
                CityId = "1",
                PhoneNumber = "0888888899",
                Photo = "/src/KFC.jpg"
            };

            var restaurantsInCity = await this.cityService.GetRestaurantsInCity("Plovdiv", "alphabetically-a-to-z");

            List<RestaurantServiceModel> actualResults = restaurantsInCity.ToList();
            List<RestaurantServiceModel> expectedResults = new List<RestaurantServiceModel>
            {
                restaurantInPlovdiv,
                restaurantInPlovdiv2
            }
            .OrderBy(r => r.Name)
            .ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Address == actualEntry.Address, errorMessagePrefix + " " + "Address is not returned properly.");
                Assert.True(expectedEntry.PhoneNumber == actualEntry.PhoneNumber, errorMessagePrefix + " " + "Phone Number is not returned properly.");
                Assert.True(expectedEntry.CityId == actualEntry.CityId, errorMessagePrefix + " " + "City Id is not returned properly.");
                Assert.True(expectedEntry.AverageRating == actualEntry.AverageRating, errorMessagePrefix + " " + "Average Rating is not returned properly.");
                Assert.True(expectedEntry.Photo == actualEntry.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRestaurantsInCity_WithDataAlphabeticallyDescending_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "CityService GetRestaurantsInCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            await Seed(context);
            this.cityService = new CityService(context);

            var restaurantInPlovdiv = new RestaurantServiceModel
            {
                Id = "1",
                Name = "Happy",
                Address = "Ivan Vazov",
                AverageRating = 10,
                CityId = "1",
                PhoneNumber = "0888888888",
                Photo = "/src/Happy.jpg"
            };

            var restaurantInPlovdiv2 = new RestaurantServiceModel
            {
                Id = "3",
                Name = "KFC",
                Address = "Vasil Levski",
                AverageRating = 7,
                CityId = "1",
                PhoneNumber = "0888888899",
                Photo = "/src/KFC.jpg"
            };

            var restaurantsInCity = await this.cityService.GetRestaurantsInCity("Plovdiv", "alphabetically-z-to-a");

            List<RestaurantServiceModel> actualResults = restaurantsInCity.ToList();
            List<RestaurantServiceModel> expectedResults = new List<RestaurantServiceModel>
            {
                restaurantInPlovdiv,
                restaurantInPlovdiv2
            }
            .OrderByDescending(r => r.Name)
            .ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Name == actualEntry.Name, errorMessagePrefix + " " + "Name is not returned properly.");
                Assert.True(expectedEntry.Address == actualEntry.Address, errorMessagePrefix + " " + "Address is not returned properly.");
                Assert.True(expectedEntry.PhoneNumber == actualEntry.PhoneNumber, errorMessagePrefix + " " + "Phone Number is not returned properly.");
                Assert.True(expectedEntry.CityId == actualEntry.CityId, errorMessagePrefix + " " + "City Id is not returned properly.");
                Assert.True(expectedEntry.AverageRating == actualEntry.AverageRating, errorMessagePrefix + " " + "Average Rating is not returned properly.");
                Assert.True(expectedEntry.Photo == actualEntry.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRestaurantsInCity_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessage = "CityService GetRestaurantsInCity() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.cityService = new CityService(context);

            var restaurantsInCity = await this.cityService.GetRestaurantsInCity("Plovdiv");
            List<RestaurantServiceModel> actualResults = restaurantsInCity.ToList();

            Assert.True(actualResults.Count == 0, errorMessage);
        }
    }
}