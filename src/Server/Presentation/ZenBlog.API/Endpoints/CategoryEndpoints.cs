using System;
using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Features.Categories;

namespace ZenBlog.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapGet("/", GetAllCategories)
          .WithName("GetAllCategories");
    }

    private static async Task<IResult> GetAllCategories(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new CategoryGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.BadRequest(result.Errors);
    }
}
