using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ZenBlog.Application.Behaviors;
using ZenBlog.Application.Features.Categories;

namespace ZenBlog.Application.Extensions;

public static class ServiceRegistrations
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddKommand(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(CategoryGetAllQuery).Assembly);
            config.AddInterceptor(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(CategoryCreateCommandValidator).Assembly);
    }
}
