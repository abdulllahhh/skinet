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
                /*if (!context.Brands.Any())
                {
                    var brandsData = File.ReadAllText("..\\Infrastructure\\Data\\SeedData\\brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.Brands.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                if (!context.Types.Any())
                {
                    var TypesData = File.ReadAllText("..\\Infrastructure\\Data\\SeedData\\types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                    foreach (var item in types)
                    {
                        context.Types.Add(item);
                    }
                    await context.SaveChangesAsync();
                }*/
                if (!context.Products.Any())
                {
                    var ProductsData = File.ReadAllText("..\\Infrastructure\\Data\\SeedData\\products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    foreach (var item in Products)
                    {
                        context.Products.Add(item);
                    }
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
