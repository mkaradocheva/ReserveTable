namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using ReserveTable.Services.Models;

    public interface IReviewService
    {
        Task<bool> CreateReview(ReviewServiceModel review);
    }
}
