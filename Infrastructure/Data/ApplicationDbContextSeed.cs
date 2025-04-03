using core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("..\\Infrastructure\\Data\\SeedData\\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products == null)
                        return;
                    context.Products.AddRange(products);
                    
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                loggerFactory.CreateLogger<ApplicationDbContextSeed>().LogError(ex.Message);
            }
        }
    }
}
