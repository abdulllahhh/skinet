﻿using API.Middleware;
using Microsoft.OpenApi.Models;

namespace API.Extentions
{
    public static class SwagerServicesExtentions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Skinet API", Version = "v1" });
                }
                );
            return services;
        }
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SliNet API v1");
                }
                );
            app.UseCors("CorsPolicy");  
            app.UseHttpsRedirection(); 
            return app;
        }
    }
}
