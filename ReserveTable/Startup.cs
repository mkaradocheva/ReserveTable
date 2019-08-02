﻿namespace ReserveTable.App
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Data;
    using Domain;
    using System.Linq;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ReserveTableDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ReserveTableUser, ReserveTableUserRole>()
    .AddEntityFrameworkStores<ReserveTableDbContext>()
    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
           "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IReservationsService, ReservationsService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITablesService, TablesService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<ReserveTableDbContext>())
                {
                    context.Database.EnsureCreated();

                    //TODO: Implement seeders

                    if (!context.Roles.Any())
                    {
                        context.Roles.Add(new ReserveTableUserRole { Name = "Admin", NormalizedName = "ADMIN" });
                        context.Roles.Add(new ReserveTableUserRole { Name = "User", NormalizedName = "USER" });
                    }

                    if (!context.Cities.Any())
                    {
                        context.Cities.Add(new City { Name = "Sofia" });
                        context.Cities.Add(new City { Name = "Plovdiv" });
                        context.Cities.Add(new City { Name = "Varna" });
                    }

                    context.SaveChanges();
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
