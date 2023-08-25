using FastReport.Data;
using FastReport.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ActeAdministratif.Controllers;
using System.Text.Json.Serialization;
using Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using ActeAdministratif.Areas.Identity.Data;
using ActeAdministratif.Data;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;

namespace UserManager
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }



        public void ConfigureServices(IServiceCollection services)
        {

            //nouvelle Ajout
            RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddMvc();

            services.AddDbContext<SNDIContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddDbContext<AuthContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddDefaultIdentity<SNDIUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AuthContext>();

            services.AddSession();




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
                app.UseExceptionHandler("/Home/Error");
                //app.UseExceptionHandler("/Errors/NoFound");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // nouveau
            app.UseFastReport();
            //app.UseFastReport();
            // fin 
            app.UseRouting();

            //app.UseMiddleware<UserMiddleware>();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }

        private string GetConnectionSting()
        {
            string connexion = this.Configuration.GetConnectionString("DefaultConnectionString");
            if (this.Environment.IsDevelopment())
            {
                connexion = this.Configuration.GetConnectionString("DefaultConnectionString");
            }

            if (this.Environment.IsStaging() || this.Environment.IsProduction())
            {
                connexion = this.Configuration.GetConnectionString("DefaultConnectionString");
            }

            return connexion;
        }

    }
}
