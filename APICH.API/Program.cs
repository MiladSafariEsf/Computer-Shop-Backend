﻿using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using APICH.API.Security;
using APICH.DAL.Repository;
using APICH.DAL;
using Microsoft.EntityFrameworkCore;
using SFM.Security;
using APICH.CORE.interfaces;
using APICH.BL.Services.Classes;
using APICH.CORE.Entity;
using Microsoft.Extensions.Configuration;

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
            builder.Services.AddScoped<IBannerService, BanerService>();
            builder.Services.AddSingleton<JwtService>();
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

            // استفاده از StaticFiles برای نمایش تصاویر و تنظیم هدرهای کش
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    OnPrepareResponse = context =>
            //    {
            //        var contentType = context.Context.Response.ContentType;
            //        if (contentType != null && contentType.StartsWith("image/"))
            //        {
            //            // تنظیم هدر Cache-Control برای تصاویر
            //            context.Context.Response.Headers["Cache-Control"] = "public, max-age=86400"; // 24 ساعت کش
            //        }
            //    }
            //});
            var configuration = builder.Configuration;
            using (var scope = app.Services.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                SeedAdminUser(userService, configuration).GetAwaiter().GetResult();
            }
            //app.UseRateLimiter();
            // Configure the HTTP request pipeline.
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
        private static async Task SeedAdminUser(IUserService userService, IConfiguration configuration)
        {
            if (await userService.GetByNumber(configuration["Owner:UserNumber"] ?? throw new ArgumentNullException("Owner:UserNumber is missing in configuration.")) == null)
            {
                var Owner = new
                {
                    UserName = configuration["Owner:UserName"] ?? throw new ArgumentNullException("Owner:UserName is missing in configuration."),
                    UserNumber = configuration["Owner:UserNumber"] ?? throw new ArgumentNullException("Owner:UserNumber is missing in configuration."),
                    Password = configuration["Owner:Password"] ?? throw new ArgumentNullException("Owner:Password is missing in configuration.")
                };
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = Owner.UserName,
                    Address = "",
                    CreatedAt = DateTime.Now,
                    HashedPassword = PasswordHasher.HashPasswordWithSalt(Owner.Password, out string salt), // در حالت واقعی هش کن
                    Number = Owner.UserNumber,
                    Salt = salt,
                    Role = Role.Owner(),
                };

                await userService.AddUser(admin);
            }
        }
    }
}
