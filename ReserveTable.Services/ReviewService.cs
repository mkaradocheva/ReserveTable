namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Data;
    using Domain;
    using Models.Reviews;

    public class ReviewService : IReviewService
    {
        private readonly ReserveTableDbContext dbContext;

        public ReviewService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateReview(CreateReviewBindingModel model, Restaurant restaurant, string userId)
        {
            var review = new Review
            {
                Rate = model.Rate,
                UserId = userId,
                User = dbContext.Users.Find(userId),
                Comment = model.Comment,
                RestaurantId = restaurant.Id,
                Restaurant = dbContext.Restaurants.Find(restaurant.Id)
            };

            await dbContext.Reviews.AddAsync(review);
            var result = await dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
