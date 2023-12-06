using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Voicecord.Data;
using Voicecord.Data.Repositories;
using Voicecord.Hubs;
using Voicecord.Interfaces;
using Voicecord.Models;
using Voicecord.Service.Implementations;
using Voicecord.Services;

namespace Voicecord
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            builder.Services.AddScoped<IBaseRepository<ApplicationUser>, UserRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<IBaseRepository<UserGroup>, GroupRepository>();
            builder.Services.AddScoped<IGroupService, GroupService>();




            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

 

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/Login");
                });
            

            builder.Services.AddSignalR();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapHub<HubRtc>("/chat"));

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Group}/{action=GetGroups}/{id?}");

            app.Run();
        }
    }
}