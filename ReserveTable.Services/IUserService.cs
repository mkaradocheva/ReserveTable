namespace ReserveTable.Services
{
    using System.Threading.Tasks;
    using Domain;

    public interface IUserService
    {
        Task<ReserveTableUser> GetUserById(string id);

        Task<ReserveTableUser> GetUserByUsername(string username);
    }
}
