using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

using APICH.DAL.Repository;
using APICH.DAL;
using APICH.BL.Services;
using Microsoft.EntityFrameworkCore;
using SFM.Security;
using APICH.BL.Services.interfaces;
namespace APICH.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<APICH_DbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBCMH"));
            });
            // اضافه کردن سرویس CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://192.168.1.103:3000")
                          .AllowAnyHeader()  // اجازه به هر هدر
                          .AllowAnyMethod()  // اجازه به هر متد
                          .AllowCredentials(); // اجازه به درخواست‌های دارای credentials
                });
            });

            //builder.Services.AddRateLimiter(options =>
            //{
            //    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            //        RateLimitPartition.GetFixedWindowLimiter(
            //            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown", // به ازای هر IP
            //            factory: _ => new FixedWindowRateLimiterOptions
            //            {
            //                PermitLimit = 60, // تعداد درخواست‌ها
            //                Window = TimeSpan.FromSeconds(30), // در هر ۳۰ ثانیه
            //                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            //                QueueLimit = 2 // تعداد درخواست‌هایی که در صف می‌مانند
            //            }
            //        ));
            //});

            var app = builder.Build();

            //app.UseRateLimiter();
            // Configure the HTTP request pipeline.
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
