using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Blogs;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class BlogEndpoints
{
    public static void MapBlogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/blogs")
            .WithTags("Blogs");

        group.MapGet("/", GetAllBlogs)
          .WithName("GetAllBlogs");

        group.MapGet("/{id:guid}", GetBlogById)
            .WithName("GetBlogById");

        group.MapPost("/", CreateBlog)
            .WithName("CreateBlog");

        group.MapPut("/{id:guid}", UpdateBlog)
            .WithName("UpdateBlog");

        group.MapDelete("/{id:guid}", DeleteBlog)
            .WithName("DeleteBlog");

    }

    private static async Task<IResult> GetAllBlogs(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new BlogGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<BlogDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<BlogDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetBlogById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new BlogGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<BlogDto>.Success(result.Data!))
            : Results.NotFound(Result<BlogDto>.Failure(new Error("Blog", "Not found.")));
    }

    private static async Task<IResult> CreateBlog(
        [FromServices] IMediator mediator,
        BlogCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/blogs/{result.Data}", Result<Guid>.Success(result.Data!))
            : Results.BadRequest(Result<Guid>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateBlog(
        [FromServices] IMediator mediator,
        Guid id,
        BlogUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new BlogUpdateCommand(id, request.Title, request.CoverImage, request.ConteBlogImagent, request.Description, request.CategoryId);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<BlogUpdateCommand>.Success(result.Data!))
            : Results.BadRequest(Result<BlogUpdateCommand>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteBlog(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new BlogDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}
 