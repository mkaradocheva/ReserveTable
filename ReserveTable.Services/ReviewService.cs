namespace ReserveTable.Services
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Models;

    public class ReviewService : IReviewService
    {
        private readonly ReserveTableDbContext dbContext;

        public ReviewService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(ReviewServiceModel reviewServiceModel)
        {
            Review review = AutoMapper.Mapper.Map<Review>(reviewServiceModel);

            if(review.Rate <= 0 || review.Rate > 10)
            {
                throw new ArgumentException(nameof(review));
            }

            if (review.Comment == string.Empty)
            {
                throw new ArgumentException(nameof(review));
            }

            await dbContext.Reviews.AddAsync(review);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
