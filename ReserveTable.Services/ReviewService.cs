namespace ReserveTable.Services
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Models;

    public class ReviewService : IReviewService
    {
        private const int MinRate = 1;
        private const int MaxRate = 10;

        private readonly ReserveTableDbContext dbContext;

        public ReviewService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(ReviewServiceModel reviewServiceModel)
        {
            Review review = AutoMapper.Mapper.Map<Review>(reviewServiceModel);
            ValidateReview(review);

            await dbContext.Reviews.AddAsync(review);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }

        private static void ValidateReview(Review review)
        {
            if (review.Rate < MinRate
                            || review.Rate > MaxRate
                            || review.Comment == string.Empty)
            {
                throw new ArgumentException(nameof(review));
            }
        }
    }
}
