
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NewsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Logging.AddConsole();
            var dbConnectionString = builder.Configuration.GetConnectionString("Database");
            var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

            if (string.IsNullOrEmpty(dbConnectionString) || string.IsNullOrEmpty(redisConnectionString)) 
            {
                throw new Exception("Connection strings are null or empty!");
            }

            builder.Services.AddDbContext<NewsDBContext>(opt =>
                opt.UseMySql(dbConnectionString,
                new MySqlServerVersion(new Version(9, 0, 0))).LogTo(Console.WriteLine, LogLevel.Debug)
                    //TODO: Remove on deploy
                    .LogTo(Console.WriteLine, LogLevel.Warning)
                    .EnableDetailedErrors());
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.Logger.LogInformation("Builded application");
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.MapControllers();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/api/errors");
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.Logger.LogInformation("Starting app! ;)");
            app.Run();
        }
    }
}
