using API.Extentions;
using API.Helpers;
using API.Middleware;
using core.Entities;
using core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)    
        {
            var builder = WebApplication.CreateBuilder(args); // Create a new web application builder

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>      // Configure the database context
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Use SQL Server with the connection string from configuration
            });
            builder.Services.AddApplicationServices(); // Add application services

            builder.Services.AddControllers(); // Add controllers to the service collection
            builder.Services.AddAutoMapper(typeof(MappingProfiles)); // Add AutoMapper with the specified mapping profiles
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer(); // Add endpoints API explorer
            builder.Services.AddSwaggerDocumentation(); // Add Swagger documentation
            builder.Services.AddSingleton<IConnectionMultiplexer>( // Register Redis connection multiplexer as a singleton
                config =>
                {
                    var connString = builder.Configuration.GetConnectionString("Redis"); // Get the Redis connection string from configuration
                    if (string.IsNullOrEmpty(connString))   // Check if the connection string is null or empty
                    {
                        throw new ArgumentNullException("Redis connection string is not configured.");
                    }
                    var options = ConfigurationOptions.Parse(connString,true); // Parse the connection string
                    return ConnectionMultiplexer.Connect(options); // Connect to Redis
                }
                );
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
            builder.Services.AddSingleton<ICartService, CartService>(); // Register the CartService as a singleton
            builder.Services.AddAuthorization(); // Add authorization services
            builder.Services.AddIdentityApiEndpoints<AppUser>() // Add identity services
                .AddEntityFrameworkStores<ApplicationDbContext>(); // Specify the DbContext to use with Identity

            var app = builder.Build(); 

            
            // Configure the HTTP request pipeline.

            app.UseDeveloperExceptionPage(); // Use developer exception page in development
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Handle status code pages
            app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
            app.UseAuthentication();
            app.UseAuthorization(); // Use authorization middleware
            try
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                await context.Database.MigrateAsync();
                await ApplicationDbContextSeed.SeedAsync(context, loggerFactory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            app.UseSwaggerDocumentation(); // Use Swagger documentation
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                .WithOrigins("http://localhost:4200", "https://localhost:4200")); // Use CORS policy
            app.MapControllers();   // Map controllers to endpoints
            app.MapGroup("api").MapIdentityApi<AppUser>(); // Map identity API endpoints api/login, api/register, etc.
            app.UseStaticFiles();   // Serve static files


            app.Run();  // Run the application
        }
    }
}