using ReserveTable.Domain;

namespace ReserveTable.Services
{
    public interface IUsersService
    {
        ReserveTableUser GetUserById(string id);
    }
}
