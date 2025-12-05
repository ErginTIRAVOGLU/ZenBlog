using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options; 
using ZenBlog.Application.Behaviors;
using ZenBlog.Application.Features.Categories;
using Microsoft.Extensions.Configuration;
using ZenBlog.Application.Options;

namespace ZenBlog.Application.Extensions;

public static class ServiceRegistrations
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKommand(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(CategoryGetAllQuery).Assembly);
            config.AddInterceptor(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CategoryCreateCommandValidator).Assembly);

        services.Configure<JwtTokenOptions>( options =>
        {
            configuration.GetSection(JwtTokenOptions.SectionName).Get<JwtTokenOptions>();
            
        });
    }

}
