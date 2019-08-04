namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Data;
    using Domain;

    public class ReviewService : IReviewService
    {
        private readonly ReserveTableDbContext dbContext;

        public ReviewService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateReview(Review review)
        {
            await dbContext.Reviews.AddAsync(review);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
