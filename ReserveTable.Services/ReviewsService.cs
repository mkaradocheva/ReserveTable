﻿using System.Threading.Tasks;
using ReserveTable.Data;
using ReserveTable.Domain;
using ReserveTable.Models.Reviews;

namespace ReserveTable.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly ReserveTableDbContext dbContext;

        public ReviewsService(ReserveTableDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateReview(CreateReviewBindingModel model, Restaurant restaurant, string userId)
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

            dbContext.Reviews.Add(review);
            dbContext.SaveChanges();
        }
    }
}
