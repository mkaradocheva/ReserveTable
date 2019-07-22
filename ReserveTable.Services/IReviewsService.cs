using System.Threading.Tasks;
using ReserveTable.Domain;
using ReserveTable.Models.Reviews;

namespace ReserveTable.Services
{
    public interface IReviewsService
    {
        void CreateReview(CreateReviewBindingModel model, Restaurant restaurant, string userId);
    }
}
