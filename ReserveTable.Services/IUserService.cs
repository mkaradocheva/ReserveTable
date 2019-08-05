namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using ReserveTable.Services.Models;

    public interface IUserService
    {
        Task<ReserveTableUserServiceModel> GetUserById(string id);

        Task<ReserveTableUserServiceModel> GetUserByUsername(string username);
    }
}
