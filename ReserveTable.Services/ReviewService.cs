namespace ReserveTable.Services
{
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

            await dbContext.Reviews.AddAsync(review);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
