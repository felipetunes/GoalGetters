using GoalGetters.Models;
using GoalGetters.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GoalGetters
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<PlayerService>();
            builder.Services.AddHttpClient<TeamService>();
            builder.Services.AddHttpClient<UserService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ApiService<Player>>();
            builder.Services.AddScoped<ApiService<Live>>();
            builder.Services.AddScoped<ApiService<User>>();
            builder.Services.AddScoped<ApiService<Bet>>();
            builder.Services.AddScoped<ApiService<Team>>();
            builder.Services.AddScoped<ApiService<Championship>>();

            builder.Services.AddScoped<IApiService<Player>, ApiService<Player>>();
            builder.Services.AddScoped<IApiService<Team>, ApiService<Team>>();

            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<ITeamService, TeamService>();


            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = "/Login";
              });

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
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
