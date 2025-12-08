using System;

namespace ZenBlog.API.Endpoints;

public static class EndpointsRegistration
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/api");//.RequireAuthorization();
        
        authGroup.MapBlogEndpoints();
        authGroup.MapCategoryEndpoints();
        authGroup.MapCommentEndpoints();
        authGroup.MapContactInfoEndpoints();
        authGroup.MapMessageEndpoints();
        authGroup.MapSocialEndpoints();
        authGroup.MapSubCommentEndpoints();
        authGroup.MapUserEndpoints();
        
        // Auth endpoints herkese açık olmalı, direkt app'a map et
        app.MapAuthEndpoints();
    }
}
