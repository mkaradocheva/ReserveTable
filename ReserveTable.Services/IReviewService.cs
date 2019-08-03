namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Domain;
    using Models.Reviews;

    public interface IReviewService
    {
        Task<bool> CreateReview(CreateReviewBindingModel model, Restaurant restaurant, string userId);
    }
}
