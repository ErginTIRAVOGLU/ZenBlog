using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Categories;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapGet("/", GetAllCategories)
          .WithName("GetAllCategories");

        group.MapGet("/{id:guid}", GetCategoryById)
            .WithName("GetCategoryById");

        group.MapPost("/", CreateCategory)
            .WithName("CreateCategory");

        group.MapPut("/{id:guid}", UpdateCategory)
            .WithName("UpdateCategory");

        group.MapDelete("/{id:guid}", DeleteCategory)
            .WithName("DeleteCategory");

    }

    private static async Task<IResult> GetAllCategories(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new CategoryGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<CategoryDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<CategoryDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetCategoryById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new CategoryGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<CategoryDto>.Success(result.Data!))
            : Results.NotFound(Result<CategoryDto>.Failure(new Error("Category", "Not found.")));
    }

    private static async Task<IResult> CreateCategory(
        [FromServices] IMediator mediator,
        CategoryCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/categories/{result.Data}", Result<CategoryDto>.Success(result.Data!))
            : Results.BadRequest(Result<CategoryDto>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateCategory(
        [FromServices] IMediator mediator,
        Guid id,
        CategoryUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new CategoryUpdateCommand(id, request.CategoryName);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<CategoryDto>.Success(result.Data!))
            : Results.BadRequest(Result<CategoryDto>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteCategory(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CategoryDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}
