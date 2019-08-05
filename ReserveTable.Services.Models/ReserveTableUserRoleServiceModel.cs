namespace ReserveTable.Services.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using ReserveTable.Domain;
    using ReserveTable.Mapping;

    public class ReserveTableUserRoleServiceModel : IdentityRole, IMapTo<ReserveTableUserRole>, IMapFrom<ReserveTableUserRole>
    {
    }
}
