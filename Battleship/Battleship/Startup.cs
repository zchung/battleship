using Battleship.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Battleship.Data.Context.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.Services;
using Battleship.Hubs;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Factories;

namespace Battleship
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
            services.AddCors(o => o.AddPolicy("AllAccess", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            //services.AddScoped<IGameDbService, GameDbService>(); // this has been commented out because heroku doesn't support local db.
            services.AddSingleton<IGameDbService, GameDbStaticService>(); // this will mock the db as a singleton class.
            services.AddScoped<IGameFactory, GameFactory>();
            services.AddScoped<IGameUpdateService, GameUpdateService>();
            services.AddScoped<IShipFactory, ShipFactory>(); 
            services.AddScoped<ICoordinatesFactory, CoordinatesFactory>();
            services.AddScoped<ICoordinatesValidationService, CoordinatesValidationService>();
            services.AddScoped<ICoordinatesUpdateService, CoordinatesUpdateService>();
            services.AddScoped<IShipUpdateService, ShipUpdateService>();
            services.AddScoped<IRandomGeneratorFactory, RandomGeneratorFactory>();
            services.AddScoped<IPlayerFactory, PlayerFactory>();
            services.AddScoped<IGameValidationService, GameValidationService>();

            services.AddDbContext<IBattleshipDbContext, BattleshipDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors("AllAccess");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<GameHub>("/game");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp/dist";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
                else
                {
                    spa.UseAngularCliServer(npmScript: "start:prod");
                }
            });
        }
    }
}
