//<<<<<<< Updated upstream
//=======
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NEXA.Data;
using NEXA.Models;
using NEXA.Repositories.IRepository;
using NEXA.Utitlies;

//>>>>>>> Stashed changes
namespace NEXA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

//<<<<<<< Updated upstream
//=======
            var connectionString =
     builder.Configuration.GetConnectionString("DefaultConnection")
         ?? throw new InvalidOperationException("Connection string"
         + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddScoped<IRepository<applicationUserOTP>, Repository<applicationUserOTP>>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();




//>>>>>>> Stashed changes
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Employee}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
