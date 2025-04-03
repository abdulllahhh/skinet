using API.DTOs;
using AutoMapper;
using core.Entities;
namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()

                .ForMember(d => d.PictureURL, o => o.MapFrom<ProductUrlResolver>());  
        }
        

    }
    
    
}
