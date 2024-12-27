using AutoMapper;
using ProductAPI.Models.DTOs;

namespace ProductAPI.Models
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
   
}
