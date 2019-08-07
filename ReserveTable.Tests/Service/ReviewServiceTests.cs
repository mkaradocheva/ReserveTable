namespace ReserveTable.Tests.Service
{
    using System;
    using System.Threading.Tasks;
    using Services;
    using Services.Models;
    using Common;
    using Xunit;
    using ReserveTable.Mapping;
    using System.Reflection;
    using AutoMapper;
    using ReserveTable.Domain;

    public class ReviewServiceTests
    {
        private IReviewService reviewService;

        public ReviewServiceTests()
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
        public async Task Create_WithInvalidRate_ShouldThrowArgumentException(double rate)
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reviewService = new ReviewService(context);

            ReviewServiceModel review = new ReviewServiceModel
            {
                Date = DateTime.Now,
                Comment = "Good",
                Rate = rate,
            };

            await Assert.ThrowsAsync<ArgumentException>(() => this.reviewService.Create(review));
        }

        [Fact]
        public async Task Create_WithNoComment_ShouldThrowArgumentException()
        {
            var context = ReserveTableDbContextInMemoryFactory.InitializeContext();
            this.reviewService = new ReviewService(context);

            ReviewServiceModel review = new ReviewServiceModel
            {
                Date = DateTime.Now,
                Comment = string.Empty,
                Rate = 9,
            };

            await Assert.ThrowsAsync<ArgumentException>(() => this.reviewService.Create(review));
        }
    }
}
