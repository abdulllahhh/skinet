using API.DTOs;
using AutoMapper;
using core.Entities;
using Microsoft.Extensions.Configuration;
namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public ProductUrlResolver(Microsoft.Extensions.Configuration.IConfiguration  config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
             if(!string.IsNullOrEmpty(source.PictureURL))
             {
                return _config["ApiUrl"] + source.PictureURL;
             }
             return "null";
        }
    }
}
