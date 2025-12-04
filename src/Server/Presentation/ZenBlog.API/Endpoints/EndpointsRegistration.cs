using System;

namespace ZenBlog.API.Endpoints;

public static class EndpointsRegistration
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCategoryEndpoints();
        app.MapBlogEndpoints();
        app.MapUserEndpoints();
    }
}
