using Mapster;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Mappings;

public sealed class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
       config.ForType<Category, CategoryDto>()
              .Ignore(dest => dest.Blogs); // Döngüyü KESEN yer
    
         config.ForType<Blog, BlogDto>()
              .Map(dest => dest.CategoryName, src => src.Category.CategoryName)
              .Map(dest => dest.UserName, src => src.User.UserName);


    }
}
