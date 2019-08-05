namespace ReserveTable.Tests.Service
{
    using System;
    using System.Threading.Tasks;
    using Services;
    using Services.Models;
    using Common;
    using Xunit;
    using ReserveTable.Data;
    using System.Linq;

    public class ReviewServiceTest
    {
        private IReviewService reviewService;

        public ReviewServiceTest()
        {
            AutoMapperFactory.InitializeMapper();
        }

        [Fact]
        public async Task Create_WithCorrectData_ShouldCreateSucessfully()
        {
            string errorMessage = "ReviewService Create() method does not work properly.";

            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reviewService = new ReviewService(context);

            ReviewServiceModel review = new ReviewServiceModel
            {
                Date = DateTime.Now,
                Comment = "Good",
                Rate = 9,
            };

            bool actualResult = await this.reviewService.Create(review);
            Assert.True(actualResult, errorMessage);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(-1)]
        public async Task Create_WithInvalidRate_ShouldNotAddInDb(double rate)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reviewService = new ReviewService(context);

            ReviewServiceModel review = new ReviewServiceModel
            {
                Date = DateTime.Now,
                Comment = "Good",
                Rate = rate,
            };

            int expectedResult = 0;
            int actualResult = context.Reviews.Count();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task Create_WithNoComment_ShouldNotAddInDb()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reviewService = new ReviewService(context);

            ReviewServiceModel review = new ReviewServiceModel
            {
                Date = DateTime.Now,
                Comment = string.Empty,
                Rate = 9,
            };

            int expectedResult = 0;
            int actualResult = context.Reviews.Count();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
