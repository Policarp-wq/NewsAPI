
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace NewsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json");
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //var options = optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 0, 0))).LogTo(Console.WriteLine, LogLevel.Debug).Options;

            builder.Services.AddDbContext<NewsDBContext>(opt =>
                opt.UseMySql("server=localhost;port=3306;database=newsdb;uid=dbmaster;password=debil",
                new MySqlServerVersion(new Version(9, 0, 0))).LogTo(Console.WriteLine, LogLevel.Debug)
                    //TODO: Remove on deploy
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            app.MapControllers();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseHttpsRedirection();
            
            //app.UseAuthorization();
            app.Run();
        }
    }
}
