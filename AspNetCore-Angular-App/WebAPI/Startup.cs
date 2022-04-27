using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Profiling.Storage;
using System;
using WebAPI.DAL;
using WebAPI.Extensions;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDependencyInjection()
                .AddSpaStaticFiles()
                .AddDbContext(Configuration)
                .ConfigureAppSettingsService(Configuration)
                .AddJwt(Configuration)
                .AddSwagger()
                .AddCors()
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddMiniProfiler(options =>
            {
                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                // (Optional) Control which SQL formatter to use, InlineFormatter is the default
                //options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                options.TrackConnectionOpenClose = false;

                // (Optional) Use something other than the "light" color scheme.
                // (defaults to "light")
                options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;

                // The below are newer options, available in .NET Core 3.0 and above:

                // (Optional) You can disable MVC filter profiling
                // (defaults to true, and filters are profiled)
                //options.EnableMvcFilterProfiling = false;

                //// (Optional) You can disable MVC view profiling
                //// (defaults to true, and views are profiled)
                //options.EnableMvcViewProfiling = false;
                //options.IgnoredPaths.Add(".js"); 
                //options.IgnoredPaths.Add("sockjs-node");
            }).AddEntityFramework();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext applicationDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseMiniProfiler()
                    .UseDeveloperExceptionPage()
                    .UseSwaggerExt()
                    .UseCorsExt(Configuration);
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts()
                    .UseSpaStaticFiles();
            }

            app.UseExceptionHandler("/Error")
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UserEndpoints()
                .UseSpa(env);

            applicationDbContext.Database.EnsureCreated();
        }
    }
}
