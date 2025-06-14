using HealthCareSystem.Models;
using HealthCareSystem.Repositories;
using HealthCareSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace HealthCareSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<HealthCareSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HealthCareDb")));

            builder.Services.AddScoped<IUserService,UserRepository>();
            builder.Services.AddScoped<IPatientService, PatientRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorRepository>();

            builder.Services.AddSession();
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
