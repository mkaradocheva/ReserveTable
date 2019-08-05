namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using ReserveTable.Services.Models;

    public interface IReviewService
    {
        Task<bool> Create(ReviewServiceModel review);
    }
}
