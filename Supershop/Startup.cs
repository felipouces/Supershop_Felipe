using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Supershop.Data;
using Supershop.Data.Entities;
using Supershop.Helpers;

namespace Supershop
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

            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true; // Ensure that each user has a unique email address
                cfg.Password.RequireDigit = false; // Passwords do not require a digit
                cfg.Password.RequiredLength = 6; // Minimum password length is 6 characters
                cfg.Password.RequireLowercase = false; // Passwords do not require a lowercase letter
                cfg.Password.RequiredUniqueChars = 0; // Passwords do not require a unique character
                cfg.Password.RequireNonAlphanumeric = false; // Passwords do not require a non-alphanumeric character
                cfg.Password.RequireUppercase = false; // Passwords do not require an uppercase letter
            })
                
                .AddEntityFrameworkStores<DataContext>() // Use the DataContext for storing user data
                ; // End of Identity configuration


            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<SeedDb>();

            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IConverterHelper, ConverterHelper>();


            // Register the repository as a service
            services.AddScoped<IProductRepository, ProductRepository>();
            // Use MockRepository for testing purposes
            //services.AddScoped<IRepository, MockRepository>(); 



            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Use authentication and authorization middleware
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
