using System;

namespace ZenBlog.API.Endpoints;

public static class EndpointsRegistration
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapBlogEndpoints();
        app.MapCategoryEndpoints();
        app.MapCommentEndpoints();
        app.MapContactInfoEndpoints();
        app.MapMessageEndpoints();
        app.MapSocialEndpoints();
        app.MapSubCommentEndpoints();
        app.MapUserEndpoints();
    }
}
