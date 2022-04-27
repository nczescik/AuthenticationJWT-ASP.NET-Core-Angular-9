using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Profiling.Storage;
using System;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DAL;
using WebAPI.DAL.Entities;
using WebAPI.DAL.Interfaces;
using WebAPI.DAL.Repositories;
using WebAPI.Extensions;
using WebAPI.Helpers;
using WebAPI.Services.Services.Users;

namespace WebAPI.Extensions
{
    public static class StartUpExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<Func<IRepository<User>>>(x => () => x.GetService<IRepository<User>>());
            services.AddScoped<Func<IRepository<Address>>>(x => () => x.GetService<IRepository<Address>>());
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("Database")));
            return services;
        }

        public static IServiceCollection ConfigureAppSettingsService(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            return services;
        }

        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<Func<IUserService>>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService().GetUserById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "v1", });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddMiniProfilerExt(this IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
                options.TrackConnectionOpenClose = false;
            }).AddEntityFramework();

            return services;
        }

        public static IApplicationBuilder UseCorsExt(this IApplicationBuilder app)
        {
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("x-miniprofiler-ids"));

            return app;
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

        public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder app)
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
                });
            return app;
        }

        public static IApplicationBuilder UseMiniProfilerExt(this IApplicationBuilder app)
        {
            //Potrzebne, aby działało na innym originie niż hostowana apka
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments(new PathString("/mini-profiler-resources"))) // Must match RouteBasePath
                {
                    if (context.Request.Headers.TryGetValue("Origin", out var origin))
                    {
                        context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                        if (context.Request.Method == "OPTIONS")
                        {
                            context.Response.StatusCode = 200;
                            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                            context.Response.Headers.Add("Access-Control-Allow-Methods", "OPTIONS, GET");
                            await context.Response.CompleteAsync();
                            return;
                        }
                    }
                }

                await next();
            }).UseMiniProfiler();

            return app;
        }
    }
}
