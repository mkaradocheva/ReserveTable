namespace ReserveTable.Services
{
    using Domain;

    public interface IUsersService
    {
        ReserveTableUser GetUserById(string id);

        ReserveTableUser GetUserByUsername(string username);
    }
}
