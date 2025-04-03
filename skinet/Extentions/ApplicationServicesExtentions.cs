using API.Errors;
using API.Helpers;
using core.Interfaces;
using Infrastructure.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace API.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                     policy.AllowAnyOrigin() // Allow Angular app
                        .AllowAnyMethod()  // Allow GET, POST, PUT, DELETE
                        .AllowAnyHeader()  // Allow all headers
                        .AllowCredentials() // Allow cookies/auth headers
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .DisallowCredentials();

                });
            });

            return services;
        }
    }
}
