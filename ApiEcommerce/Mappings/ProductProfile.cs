using System;
using ApiEcommerce.Model;
using AutoMapper;

namespace ApiEcommerce.Mappings;

public class ProductProfile : Profile
{
  public ProductProfile()
  {
    CreateMap<Product, ProductDto>().ReverseMap();
    CreateMap<Product, CreateProductDto>().ReverseMap();
    CreateMap<Product, UpdateProductDto>().ReverseMap();
  }

}
