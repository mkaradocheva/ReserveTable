namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Domain;

    public interface IReviewService
    {
        Task<bool> CreateReview(Review review);
    }
}
