namespace ReserveTable.Services
{
    using Domain;
    using Models.Reviews;

    public interface IReviewsService
    {
        void CreateReview(CreateReviewBindingModel model, Restaurant restaurant, string userId);
    }
}
