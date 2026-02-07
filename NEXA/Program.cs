//<<<<<<< Updated upstream
//=======
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NEXA.Data;
using NEXA.Models;
using NEXA.Repositories.IRepository;
using System.Numerics;
using NEXA.Utitlies;
using NEXA.Repositories;

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





            builder.Services.AddScoped<IRepository<Answer>, Repository<Answer>>();
            builder.Services.AddScoped<IRepository<Certificate>, Repository<Certificate>>();
            builder.Services.AddScoped<IRepository<Course>, Repository<Course>>();
            builder.Services.AddScoped<IRepository<Enrollment>, Repository<Enrollment>>();
            builder.Services.AddScoped<IRepository<Exam>, Repository<Exam>>();
            builder.Services.AddScoped<IRepository<ExamResult>, Repository<ExamResult>>();
            builder.Services.AddScoped<IRepository<Lesson>, Repository<Lesson>>();
            builder.Services.AddScoped<IRepository<Module>, Repository<Module>>();
            builder.Services.AddScoped<IRepository<Progress>, Repository<Progress>>();
            builder.Services.AddScoped<IRepository<Question>, Repository<Question>>();
            builder.Services.AddScoped<IRepository<Resource>, Repository<Resource>>();


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
