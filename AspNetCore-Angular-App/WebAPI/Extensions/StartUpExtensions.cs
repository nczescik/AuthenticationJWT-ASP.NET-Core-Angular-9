using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebAPI.DAL;
using WebAPI.DAL.Interfaces;
using WebAPI.DAL.Repositories;
using WebAPI.Extensions;
using WebAPI.Services.Services.Users;

namespace WebAPI.Extensions
{
    public static class StartUpExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<Func<IUserService>>(x => () => x.GetService<IUserService>());
            return services;
        }

        public static IServiceCollection AddSpaStaticFiles(this IServiceCollection services)
        {
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));
            return services;
        }


        public static IApplicationBuilder UserEndpoints(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            return app;
        }

        public static IApplicationBuilder UseSpa(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            return app;
        }
    }
}
