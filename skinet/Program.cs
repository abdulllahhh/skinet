using API.Extentions;
using API.Helpers;
using API.Middleware;
using core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace skinet
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                config =>
                {
                    var connString = builder.Configuration.GetConnectionString("Redis");
                    if (string.IsNullOrEmpty(connString))
                    {
                        throw new ArgumentNullException("Redis connection string is not configured.");
                    }
                    var options = ConfigurationOptions.Parse(connString,true);
                    return ConnectionMultiplexer.Connect(options);
                }
                );
            builder.Services.AddSingleton<ICartService, CartService>();
            var app = builder.Build();

            
            // Configure the HTTP request pipeline.

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                await context.Database.MigrateAsync();
                await ApplicationDbContextSeed.SeedAsync(context, loggerFactory);
            }

            app.UseSwaggerDocumentation();
            app.MapControllers();
            app.UseStaticFiles();


            app.Run();
        }
    }
}