using System;
using Microsoft.Extensions.DependencyInjection;
using ZenBlog.Application.Features.Categories;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Extensions;

public static class ServiceRegistrations
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddKommand(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(CategoryGetAllQuery).Assembly);
        });
    }
}
