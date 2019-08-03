using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ReserveTable.App.Areas.Identity.IdentityHostingStartup))]
namespace ReserveTable.App.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}